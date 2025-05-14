using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

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
            List<Exception> allExceptions = new List<Exception>();
            
            foreach (var service in ipServices)
            {
                try
                {
                    // Imposta un timeout per evitare blocchi
                    var task = client.GetStringAsync(service);
                    if (Task.WhenAny(task, Task.Delay(5000)).Result == task) // 5 secondi di timeout
                    {
                        var response = task.Result.Trim();
                        if (!string.IsNullOrEmpty(response))
                        {
                            logger.Log($"Got IP {response} from {service}");
                            return response;
                        }
                        else
                        {
                            logger.Log($"Received empty response from {service}");
                        }
                    }
                    else
                    {
                        logger.Log($"Timeout when connecting to {service}");
                        allExceptions.Add(new TimeoutException($"Timeout when connecting to {service}"));
                    }
                }
                catch (Exception ex)
                {
                    logger.Log($"Failed to get IP from {service}: {ex.Message}");
                    allExceptions.Add(ex);
                }
            }

            // Se i servizi di IP online falliscono, prova a ottenere un indirizzo IP locale
            try
            {
                var localIp = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName())
                    .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString();
                
                if (!string.IsNullOrEmpty(localIp))
                {
                    logger.Log($"Using local IP address as fallback: {localIp}");
                    return localIp;
                }
            }
            catch (Exception ex)
            {
                logger.Log($"Failed to get local IP address: {ex.Message}");
                allExceptions.Add(ex);
            }

            throw new AggregateException("Failed to get public IP address from any service", allExceptions);
        }
    }
}
