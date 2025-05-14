using System;
using System.ServiceProcess;
using System.Timers;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CloudflareDDNService
{
    public partial class CloudflareDDNService : ServiceBase
    {
        private System.Timers.Timer updateTimer;
        private CloudflareClient cloudflareClient;
        private IpAddressProvider ipProvider;
        private ConfigurationManager configManager;
        private Logger logger;
        private SystemTrayIcon trayIcon;
        private ManualResetEvent stopEvent = new ManualResetEvent(false);
        private bool isInitialized = false;

        public CloudflareDDNService()
        {
            this.ServiceName = "CloudflareDDNService";
            this.CanStop = true;
            this.CanPauseAndContinue = false;
            this.AutoLog = true;
        }

        public void SetTrayIcon(SystemTrayIcon icon)
        {
            this.trayIcon = icon;
        }

        // Nuovo metodo che può essere chiamato sia dal servizio che dall'applicazione
        public void Initialize()
        {
            if (isInitialized)
                return;

            try
            {
                // Inizializza il logger per prima cosa per poter registrare eventuali errori
                try
                {
                    logger = new Logger();
                    logger.Log("Service initializing...");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Errore durante l'inizializzazione del logger: {ex.Message}", 
                        "Errore critico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw; // Rilancia l'eccezione in quanto è un errore critico
                }

                try
                {
                    configManager = new ConfigurationManager();
                    var config = configManager.LoadConfiguration();

                    ipProvider = new IpAddressProvider();

                    // Verifica che la configurazione contenga i valori necessari
                    if (string.IsNullOrEmpty(config.ApiKey) ||
                        string.IsNullOrEmpty(config.Email) ||
                        string.IsNullOrEmpty(config.Domain))
                    {
                        logger.Log("Configurazione incompleta. Impostare API Key, Email e Domain.");
                        if (trayIcon != null)
                        {
                            trayIcon.ShowNotification(
                                "Configurazione incompleta",
                                "Apri la configurazione e imposta API Key, Email e Domain.",
                                true);
                        }

                        // Non inizializzare il client e non eseguire aggiornamenti DNS
                        // ma inizializza comunque il timer per poter avviare il servizio
                        updateTimer = new System.Timers.Timer(60000); // Controlla ogni minuto
                        updateTimer.Elapsed += (s, e) => CheckConfiguration();
                        updateTimer.Start();

                        isInitialized = true;
                        return;
                    }

                    // Se la configurazione è completa, inizializza il client
                    cloudflareClient = new CloudflareClient(config.ApiKey, config.Email, config.Domain);

                    // Inizializza il timer
                    int interval = Math.Max(1, config.UpdateInterval) * 60 * 1000; // Minimo 1 minuto
                    updateTimer = new System.Timers.Timer(interval);
                    updateTimer.Elapsed += OnTimerElapsed;
                    updateTimer.Start();

                    logger.Log("Service initialized successfully");
                    isInitialized = true;
                }
                catch (Exception ex)
                {
                    logger.Log($"Errore durante la configurazione del servizio: {ex.Message}");
                    logger.Log($"Stack trace: {ex.StackTrace}");

                    if (trayIcon != null)
                    {
                        trayIcon.ShowNotification(
                            "Errore di configurazione",
                            $"Si è verificato un errore: {ex.Message}",
                            true);
                    }
                                        
                    isInitialized = true;
                    return;
                }

                // Esegui un aggiornamento immediato all'avvio solo se abbiamo una configurazione valida
                try
                {
                    if (cloudflareClient != null)
                    {
                        UpdateDnsRecords();
                    }
                }
                catch (Exception ex)
                {
                    logger.Log($"Errore durante l'aggiornamento DNS iniziale: {ex.Message}");
                    if (trayIcon != null)
                    {
                        trayIcon.ShowNotification(
                            "Errore di inizializzazione",
                            $"Errore durante l'aggiornamento DNS: {ex.Message}",
                            true);
                    }
                }
            }
            catch (Exception ex)
            {
                if (logger != null)
                {
                    logger.Log($"Errore di inizializzazione del servizio: {ex.Message}");
                    logger.Log($"Stack trace: {ex.StackTrace}");
                }

                if (trayIcon != null)
                {
                    trayIcon.ShowNotification(
                        "Errore di inizializzazione",
                        $"Si è verificato un errore: {ex.Message}",
                        true);
                }
                else 
                {
                    MessageBox.Show($"Errore di inizializzazione del servizio: {ex.Message}\n\n{ex.StackTrace}", 
                        "Errore critico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Segniamo come inizializzato per evitare loop infiniti
                isInitialized = true;
            }
        }

        private void CheckConfiguration()
        {
            try
            {
                var config = configManager.LoadConfiguration();

                if (!string.IsNullOrEmpty(config.ApiKey) &&
                    !string.IsNullOrEmpty(config.Email) &&
                    !string.IsNullOrEmpty(config.Domain))
                {
                    // La configurazione è stata completata, reinizializza il servizio
                    logger.Log("Configurazione completata, reinizializzazione del servizio.");

                    // Ferma il timer di controllo
                    updateTimer.Stop();

                    // Reinizializza il client e il timer
                    cloudflareClient = new CloudflareClient(config.ApiKey, config.Email, config.Domain);

                    int interval = Math.Max(1, config.UpdateInterval) * 60 * 1000;
                    updateTimer = new System.Timers.Timer(interval);
                    updateTimer.Elapsed += OnTimerElapsed;
                    updateTimer.Start();

                    // Esegui un aggiornamento immediato
                    UpdateDnsRecords();
                }
            }
            catch (Exception ex)
            {
                logger.Log($"Errore durante il controllo della configurazione: {ex.Message}");
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                logger = new Logger();
                logger.Log("Service starting...");

                Initialize();

                logger.Log("Service started successfully");
            }
            catch (Exception ex)
            {
                if (logger != null)
                {
                    logger.Log($"Errore durante l'avvio del servizio: {ex.Message}");
                    logger.Log($"Stack trace: {ex.StackTrace}");
                }
                
                throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                logger.Log("Service stopping...");

                if (updateTimer != null)
                    updateTimer.Stop();

                if (trayIcon != null)
                    trayIcon.Dispose();

                stopEvent.Set();
                logger.Log("Service stopped");
            }
            catch (Exception ex)
            {
                if (logger != null)
                {
                    logger.Log($"Errore durante l'arresto del servizio: {ex.Message}");
                }
            }
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                UpdateDnsRecords();
            }
            catch (Exception ex)
            {
                logger.Log($"Errore nell'elaborazione del timer: {ex.Message}");
            }
        }

        // Aggiungi questo metodo alla classe CloudflareDDNService
        public void ReloadConfiguration()
        {
            try
            {
                logger?.Log("Reloading configuration...");

                // Ricarica la configurazione
                var config = configManager.LoadConfiguration();

                // Se il cliente è già inizializzato, dobbiamo solo aggiornare il timer
                if (cloudflareClient != null)
                {
                    // Aggiorna il cliente con le nuove credenziali
                    cloudflareClient = new CloudflareClient(config.ApiKey, config.Email, config.Domain);

                    // Aggiorna il timer con il nuovo intervallo
                    if (updateTimer != null)
                    {
                        updateTimer.Stop();

                        // Calcola il nuovo intervallo in millisecondi
                        int interval = Math.Max(1, config.UpdateInterval) * 60 * 1000; // Minimo 1 minuto
                        updateTimer.Interval = interval;

                        // Registra il nuovo intervallo
                        logger?.Log($"Timer interval updated: {config.UpdateInterval} minutes");

                        updateTimer.Start();
                    }
                }
                else
                {
                    // Se il cliente non è ancora inizializzato, inizializza tutto
                    Initialize();
                }
            }
            catch (Exception ex)
            {
                logger?.Log($"Error reloading configuration: {ex.Message}");
                throw;
            }
        }


        public void UpdateDnsRecords()
        {
            // Verifica se il client è inizializzato
            if (cloudflareClient == null)
            {
                logger?.Log("Impossibile aggiornare i DNS: client non inizializzato");
                trayIcon?.ShowNotification(
                    "Errore di configurazione",
                    "Apri la configurazione e imposta API Key, Email e Domain.",
                    true);
                return;
            }

            try
            {
                logger.Log("╔══════════════════════════════════════════════════");
                logger.Log("║ INIZIANDO IL PROCESSO DI AGGIORNAMENTO DNS");
                logger.Log("╠══════════════════════════════════════════════════");

                string currentIp = ipProvider.GetPublicIpAddress();
                logger.Log($"║ IP pubblico attuale: {currentIp}");
          
                var result = cloudflareClient.UpdateDnsRecords(currentIp);
                var updatedRecords = cloudflareClient.GetUpdatedRecords();

                // Conta quanti record sono stati effettivamente aggiornati
                var actuallyUpdated = updatedRecords.Count(r => r.Status == "Updated");
                var unchanged = updatedRecords.Count(r => r.Status == "No change needed");
                var failed = updatedRecords.Count(r => r.Status.StartsWith("Failed"));

                if (result.Success)
                {
                    logger.Log($"║ Risultato: SUCCESSO");
                    logger.Log($"║ Record totali controllati: {updatedRecords.Count}");
                    logger.Log($"║ Record aggiornati: {actuallyUpdated}");
                    logger.Log($"║ Record già aggiornati: {unchanged}");
                    logger.Log($"║ Record non aggiornati a causa di errori: {failed}");

                    if (updatedRecords.Count > 0)
                    {
                        logger.Log("╠══════════════════════════════════════════════════");
                        logger.Log("║ DETTAGLIO DEI RECORD DNS CONTROLLATI:");

                        foreach (var record in updatedRecords)
                        {
                            string statusIcon = record.Status == "Updated" ? "✅" :
                                              record.Status == "No change needed" ? "ℹ️" : "❌";

                            logger.Log($"║ {statusIcon} {record.Name}");
                            logger.Log($"║   - Vecchio IP: {record.OldIp}");
                            logger.Log($"║   - Nuovo IP: {record.NewIp}");
                            logger.Log($"║   - Stato: {record.Status}");
                            logger.Log("║   ------------------------------------");
                        }

                        string notificationMessage = actuallyUpdated > 0
                            ? $"{actuallyUpdated} record DNS aggiornati correttamente"
                            : "Nessun record necessitava di aggiornamento";

                        if (trayIcon != null)
                            trayIcon.ShowNotification("Cloudflare DDNS", notificationMessage);
                    }
                    else
                    {
                        logger.Log("║ Nessun record DNS trovato da controllare");
                        if (trayIcon != null)
                            trayIcon.ShowNotification("Cloudflare DDNS", "Nessun record DNS trovato");
                    }
                }
                else
                {
                    logger.Log($"║ Risultato: FALLIMENTO");
                    logger.Log($"║ Motivo: {result.Message}");
                    if (trayIcon != null)
                        trayIcon.ShowNotification("Cloudflare DDNS", $"Aggiornamento fallito: {result.Message}", true);
                }

                logger.Log("╚══════════════════════════════════════════════════");
            }
            catch (Exception ex)
            {
                logger.Log("╔══════════════════════════════════════════════════");
                logger.Log("║ ERRORE DURANTE L'AGGIORNAMENTO DNS");
                logger.Log($"║ Messaggio: {ex.Message}");
                logger.Log($"║ Stack trace: {ex.StackTrace}");
                logger.Log("╚══════════════════════════════════════════════════");
                if (trayIcon != null)
                    trayIcon.ShowNotification("Cloudflare DDNS", $"Errore: {ex.Message}", true);
            }
        }
    }
}
