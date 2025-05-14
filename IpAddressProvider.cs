using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudflareDDNService
{
    public class IpAddressProvider
    {
        private readonly HttpClient client;
        private readonly Logger logger;
        private readonly string[] ipServices = 
        {
            "https://ipv4.icanhazip.com/",
            "https://api.ipify.org"
        };

        public IpAddressProvider()
        {
            client = new HttpClient();
            logger = new Logger();
        }

        public string GetPublicIpAddress()
        {
            foreach (var service in ipServices)
            {
                try
                {
                    var response = client.GetStringAsync(service).Result;
                    var ip = response.Trim();
                    logger.Log($"Got IP {ip} from {service}");
                    return ip;
                }
                catch (Exception ex)
                {
                    logger.Log($"Failed to get IP from {service}: {ex.Message}");
                }
            }

            throw new Exception("Failed to get public IP address from any service");
        }
    }
}
