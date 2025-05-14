using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace CloudflareDDNService
{
    public class SystemTrayIcon : IDisposable
    {
        private NotifyIcon notifyIcon;
        private ConfigurationForm configForm;
        private readonly CloudflareDDNService service;

        public SystemTrayIcon(CloudflareDDNService service)
        {
            this.service = service;
        }

        public void Initialize()
        {
            try
            {
                string iconPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "cloudflare.ico");
                Icon icon = null;
                
                if (File.Exists(iconPath))
                {
                    icon = new Icon(iconPath);
                }
                else
                {
                    // Usa un'icona predefinita di sistema se il file non esiste
                    icon = SystemIcons.Application;
                }
                
                notifyIcon = new NotifyIcon
                {
                    Icon = icon,
                    Visible = true,
                    Text = "Cloudflare DDNS Service"
                };

                // Create context menu
                var contextMenu = new ContextMenuStrip();
                
                var configItem = new ToolStripMenuItem("Configuration");
                configItem.Click += (s, e) => ShowConfigurationForm();
                
                var updateItem = new ToolStripMenuItem("Update DNS Now");
                updateItem.Click += (s, e) => service.UpdateDnsRecords();
                
                var exitItem = new ToolStripMenuItem("Exit");
                exitItem.Click += (s, e) => 
                {
                    // Chiudi correttamente l'applicazione
                    notifyIcon.Visible = false;
                    Application.Exit();
                };
                
                contextMenu.Items.Add(configItem);
                contextMenu.Items.Add(updateItem);
                contextMenu.Items.Add(new ToolStripSeparator());
                contextMenu.Items.Add(exitItem);
                
                notifyIcon.ContextMenuStrip = contextMenu;
                
                // Double-click to open configuration
                notifyIcon.DoubleClick += (s, e) => ShowConfigurationForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante l'inizializzazione dell'icona: {ex.Message}", 
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // Tenta di creare comunque la notifyIcon senza l'icona personalizzata
                notifyIcon = new NotifyIcon
                {
                    Icon = SystemIcons.Application,
                    Visible = true,
                    Text = "Cloudflare DDNS Service"
                };
            }
        }

        private void ShowConfigurationForm()
        {
            if (configForm == null || configForm.IsDisposed)
            {
                configForm = new ConfigurationForm();
                configForm.Show();
            }
            else
            {
                configForm.BringToFront();
            }
        }

        public void ShowNotification(string title, string message, bool isError = false)
        {
            notifyIcon.ShowBalloonTip(
                3000, 
                title, 
                message, 
                isError ? ToolTipIcon.Error : ToolTipIcon.Info);
        }

        public void Dispose()
        {
            if (notifyIcon != null)
            {
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
                notifyIcon = null;
            }

            if (configForm != null && !configForm.IsDisposed)
            {
                configForm.Close();
                configForm.Dispose();
                configForm = null;
            }
        }
    }
}
