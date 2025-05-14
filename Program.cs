using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;
using System.Threading;
using System.Reflection;

namespace CloudflareDDNService
{
    public class CloudflareApplicationContext : ApplicationContext
    {
        private readonly CloudflareDDNService service;
        private readonly SystemTrayIcon trayIcon;

        public CloudflareApplicationContext()
        {
            // Crea il servizio e la tray icon
            service = new CloudflareDDNService();
            trayIcon = new SystemTrayIcon(service);
            
            // Li connette
            service.SetTrayIcon(trayIcon);

            // Inizializazione della tray icon e del servizio
            trayIcon.Initialize();
            service.Initialize();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                trayIcon?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    static class Program
    {
        // Mutex per garantire un'unica istanza
        private static Mutex mutex = null;

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                // Controllo delle istanze multiple
                string appName = Assembly.GetExecutingAssembly().GetName().Name;
                bool createdNew;
                mutex = new Mutex(true, appName, out createdNew);
                
                if (!createdNew)
                {
                    MessageBox.Show("Un'altra istanza dell'applicazione è già in esecuzione.", 
                        "Istanza multipla", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Se l'argomento è "--service", esegui come servizio
                if (args.Length > 0 && args[0].ToLower() == "--service")
                {
                    ServiceBase[] ServicesToRun = new ServiceBase[]
                    {
                        new CloudflareDDNService()
                    };
                    ServiceBase.Run(ServicesToRun);
                }
                else
                {             
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    // Usa un ApplicationContext per gestire in modo pulito il ciclo di vita dell'applicazione
                    Application.Run(new CloudflareApplicationContext());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Si è verificato un errore durante l'avvio dell'applicazione:\n\n{ex.Message}\n\n{ex.StackTrace}",
                    "Errore di Avvio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                mutex?.ReleaseMutex();
            }
        }
    }
}
