using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CloudflareDDNService
{
    internal static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
using System;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Security.Principal;

namespace CloudflareDDNService
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Check if running as administrator
            if (!IsRunningAsAdministrator())
            {
                MessageBox.Show("This application requires administrator privileges to run properly.", 
                    "Administrator Rights Required", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
            }

            // If running with --service argument, run as a service
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
                // Otherwise, run as a Windows Forms application
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                // Create and run the system tray application
                var service = new CloudflareDDNService();
                var trayIcon = new SystemTrayIcon(service);
                trayIcon.Initialize();
                
                Application.Run();
            }
        }

        static bool IsRunningAsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
