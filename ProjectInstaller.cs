using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace CloudflareDDNService
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public ProjectInstaller()
        {
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            
            serviceInstaller.ServiceName = "CloudflareDDNService";
            serviceInstaller.DisplayName = "Cloudflare DDNS Service";
            serviceInstaller.Description = "Updates Cloudflare DNS A records with the current public IP address";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
