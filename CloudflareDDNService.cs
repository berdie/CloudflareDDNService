using System;
using System.ServiceProcess;
using System.Timers;
using System.Threading;
using System.IO;

namespace CloudflareDDNService
{
    public partial class CloudflareDDNService : ServiceBase
    {
        private Timer updateTimer;
        private CloudflareClient cloudflareClient;
        private IpAddressProvider ipProvider;
        private ConfigurationManager configManager;
        private Logger logger;
        private SystemTrayIcon trayIcon;
        private ManualResetEvent stopEvent = new ManualResetEvent(false);

        public CloudflareDDNService()
        {
            InitializeComponent();
            this.ServiceName = "CloudflareDDNService";
            this.CanStop = true;
            this.CanPauseAndContinue = false;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            logger = new Logger();
            logger.Log("Service starting...");
            
            configManager = new ConfigurationManager();
            var config = configManager.LoadConfiguration();
            
            ipProvider = new IpAddressProvider();
            cloudflareClient = new CloudflareClient(config.ApiKey, config.Email, config.Domain);
            
            // Inizializza il timer
            updateTimer = new Timer(config.UpdateInterval * 60 * 1000); // Converti minuti in millisecondi
            updateTimer.Elapsed += OnTimerElapsed;
            updateTimer.Start();
            
            // Inizializza l'icona nella system tray
            trayIcon = new SystemTrayIcon(this);
            trayIcon.Initialize();
            
            logger.Log("Service started successfully");
        }

        protected override void OnStop()
        {
            logger.Log("Service stopping...");
            updateTimer.Stop();
            trayIcon.Dispose();
            stopEvent.Set();
            logger.Log("Service stopped");
        }
        
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            UpdateDnsRecords();
        }
        
        public void UpdateDnsRecords()
        {
            try
            {
                logger.Log("Starting DNS update process");
                string currentIp = ipProvider.GetPublicIpAddress();
                logger.Log($"Current public IP: {currentIp}");
                
                var result = cloudflareClient.UpdateDnsRecords(currentIp);
                if (result.Success)
                {
                    logger.Log("DNS records updated successfully");
                    trayIcon.ShowNotification("Cloudflare DDNS", "DNS records updated successfully");
                }
                else
                {
                    logger.Log($"Failed to update DNS records: {result.Message}");
                    trayIcon.ShowNotification("Cloudflare DDNS", $"Update failed: {result.Message}", true);
                }
            }
            catch (Exception ex)
            {
                logger.Log($"Error updating DNS records: {ex.Message}");
                trayIcon.ShowNotification("Cloudflare DDNS", $"Error: {ex.Message}", true);
            }
        }
    }
}
