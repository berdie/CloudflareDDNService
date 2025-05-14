using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloudflareDDNService
{
    public class CloudflareClient
    {
        private readonly string apiKey;
        private readonly string email;
        private readonly string domain;
        private readonly HttpClient client;
        private readonly Logger logger;
        private string zoneId;
        private string recordId;

        public CloudflareClient(string apiKey, string email, string domain)
        {
            this.apiKey = apiKey;
            this.email = email;
            this.domain = domain;
            this.logger = new Logger();
            
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Auth-Email", email);
            client.DefaultRequestHeaders.Add("X-Auth-Key", apiKey);
            client.DefaultRequestHeaders.Add("Content-Type", "application/json");
        }

        public async Task<string> GetZoneIdAsync()
        {
            if (!string.IsNullOrEmpty(zoneId))
                return zoneId;

            try
            {
                var response = await client.GetAsync($"https://api.cloudflare.com/client/v4/zones?name={domain}");
                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);

                if (json["success"].Value<bool>() && json["result"].HasValues)
                {
                    zoneId = json["result"][0]["id"].Value<string>();
                    return zoneId;
                }
                
                logger.Log($"Failed to get zone ID: {content}");
                return null;
            }
            catch (Exception ex)
            {
                logger.Log($"Error getting zone ID: {ex.Message}");
                return null;
            }
        }

        public (bool Success, string Message) UpdateDnsRecords(string newIp)
        {
            try
            {
                var zoneId = GetZoneIdAsync().Result;
                if (string.IsNullOrEmpty(zoneId))
                    return (false, "Failed to get zone ID");

                // Get all A records
                var recordsResponse = client.GetAsync($"https://api.cloudflare.com/client/v4/zones/{zoneId}/dns_records?type=A").Result;
                var recordsContent = recordsResponse.Content.ReadAsStringAsync().Result;
                var recordsJson = JObject.Parse(recordsContent);

                if (!recordsJson["success"].Value<bool>())
                    return (false, $"Failed to get DNS records: {recordsContent}");

                var records = recordsJson["result"].ToObject<JArray>();
                bool anyUpdated = false;

                foreach (var record in records)
                {
                    string recordId = record["id"].Value<string>();
                    string currentIp = record["content"].Value<string>();
                    string recordName = record["name"].Value<string>();

                    if (currentIp == newIp)
                    {
                        logger.Log($"Record {recordName} already has correct IP: {newIp}");
                        continue;
                    }

                    // Update the record
                    var updateData = new
                    {
                        type = "A",
                        name = recordName,
                        content = newIp,
                        ttl = record["ttl"].Value<int>(),
                        proxied = record["proxied"].Value<bool>()
                    };

                    var json = JsonConvert.SerializeObject(updateData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var updateResponse = client.PutAsync($"https://api.cloudflare.com/client/v4/zones/{zoneId}/dns_records/{recordId}", content).Result;
                    var updateContent = updateResponse.Content.ReadAsStringAsync().Result;
                    var updateJson = JObject.Parse(updateContent);

                    if (updateJson["success"].Value<bool>())
                    {
                        logger.Log($"Updated record {recordName} from {currentIp} to {newIp}");
                        anyUpdated = true;
                    }
                    else
                    {
                        logger.Log($"Failed to update record {recordName}: {updateContent}");
                    }
                }

                if (anyUpdated)
                    return (true, "DNS records updated successfully");
                else
                    return (true, "No DNS records needed updating");
            }
            catch (Exception ex)
            {
                logger.Log($"Error updating DNS records: {ex.Message}");
                return (false, ex.Message);
            }
        }
    }
}
