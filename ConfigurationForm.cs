using System;
using System.IO;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace CloudflareDDNService
{
    public partial class ConfigurationForm : MaterialForm
    {
        private readonly ConfigurationManager configManager;
        private readonly IpAddressProvider ipProvider;
        private readonly Logger logger;
        private readonly CloudflareDDNService service; // Riferimento al servizio

        public ConfigurationForm()
        {
            InitializeComponent();

            // Setup MaterialSkin
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Orange800,
                Primary.Orange900,
                Primary.Orange500,
                Accent.Orange200,
                TextShade.WHITE);

            configManager = new ConfigurationManager();
            ipProvider = new IpAddressProvider();
            logger = new Logger();

            // Ottieni un riferimento al servizio in esecuzione dall'application context
            if (Program.ServiceInstance != null)
            {
                service = Program.ServiceInstance;
            }

            LoadConfiguration();
            UpdateCurrentIp();
        }

        private void LoadConfiguration()
        {
            var config = configManager.LoadConfiguration();

            txtApiKey.Text = config.ApiKey;
            txtEmail.Text = config.Email;
            txtDomain.Text = config.Domain;

            // Converti il valore intero in stringa
            numUpdateInterval.Text = config.UpdateInterval.ToString();
        }

        private void UpdateCurrentIp()
        {
            try
            {
                string ip = ipProvider.GetPublicIpAddress();
                lblCurrentIp.Text = $"Current IP: {ip}";
            }
            catch (Exception ex)
            {
                lblCurrentIp.Text = "Failed to get current IP";
                logger.Log($"Error getting current IP: {ex.Message}");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Converti il testo in un intero
            int updateInterval;
            if (!int.TryParse(numUpdateInterval.Text, out updateInterval))
            {
                MessageBox.Show("L'intervallo di aggiornamento deve essere un numero intero valido.",
                    "Errore di validazione", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var config = new Configuration
            {
                ApiKey = txtApiKey.Text,
                Email = txtEmail.Text,
                Domain = txtDomain.Text,
                UpdateInterval = updateInterval
            };

            configManager.SaveConfiguration(config);

            // Notifica al servizio che la configurazione è cambiata
            if (service != null)
            {
                try
                {
                    // Aggiorna la configurazione del servizio
                    service.ReloadConfiguration();
                    logger.Log("Configuration updated and service notified");
                    MessageBox.Show("Configurazione salvata e timer aggiornato",
                        "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    logger.Log($"Error updating service configuration: {ex.Message}");
                    MessageBox.Show($"Configurazione salvata, ma errore nell'aggiornare il servizio: {ex.Message}",
                        "Avviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Configuration saved successfully. Restart the application for the changes to take effect.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnUpdateNow_Click(object sender, EventArgs e)
        {
            if (service != null)
            {
                try
                {
                    service.UpdateDnsRecords();
                    MessageBox.Show("Aggiornamento DNS avviato manualmente",
                        "Aggiornamento", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    logger.Log($"Error triggering DNS update: {ex.Message}");
                    MessageBox.Show($"Errore nell'avvio dell'aggiornamento: {ex.Message}",
                        "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Impossibile contattare il servizio",
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRefreshIp_Click(object sender, EventArgs e)
        {
            UpdateCurrentIp();
        }

        private void btnViewLogs_Click(object sender, EventArgs e)
        {
            try
            {
                string logPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "CloudflareDDNService",
                    "Logs",
                    "service.log");

                if (File.Exists(logPath))
                {
                    // Apre il file di log con l'applicazione predefinita per i file .log
                    System.Diagnostics.Process.Start(logPath);
                }
                else
                {
                    MessageBox.Show("Il file di log non esiste ancora.",
                        "File non trovato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore nell'apertura del file di log: {ex.Message}",
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log($"Errore nell'apertura del file di log: {ex.Message}");
            }
        }
    }
}
