using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http.Headers;

namespace CloudflareDDNService
{
    public class DnsRecordUpdate
    {
        public string Name { get; set; }
        public string OldIp { get; set; }
        public string NewIp { get; set; }
        public string Status { get; set; }
    }

    public class CloudflareClient
    {
        private readonly string apiKey;
        private readonly string email;
        private readonly string domain;
        private readonly HttpClient client;
        private readonly Logger logger;
        private string zoneId;
        private List<DnsRecordUpdate> updatedRecords;

        public CloudflareClient(string apiKey, string email, string domain)
        {
            this.apiKey = apiKey;
            this.email = email;
            this.domain = domain;
            this.logger = new Logger();
            this.updatedRecords = new List<DnsRecordUpdate>();

            client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30); // Imposta un timeout ragionevole

            // Correggi le intestazioni HTTP
            client.DefaultRequestHeaders.Add("X-Auth-Email", email);
            client.DefaultRequestHeaders.Add("X-Auth-Key", apiKey);

            // Il Content-Type non dovrebbe essere impostato nelle DefaultRequestHeaders
            // ma nell'oggetto HttpContent quando si effettua una richiesta
        }

        public List<DnsRecordUpdate> GetUpdatedRecords()
        {
            return updatedRecords;
        }

        // Metodo per ottenere lo ZoneId in modo sincrono senza deadlock
        public string GetZoneId()
        {
            if (!string.IsNullOrEmpty(zoneId))
                return zoneId;

            try
            {
                // Utilizziamo Task.Run per eseguire il codice asincrono in modo sincrono
                // senza rischio di deadlock sul thread UI
                return Task.Run(async () => await GetZoneIdInternalAsync()).Result;
            }
            catch (Exception ex)
            {
                logger.Log($"Error getting zone ID: {ex.Message}");
                return null;
            }
        }

        private async Task<string> GetZoneIdInternalAsync()
        {
            try
            {
                var response = await client.GetAsync($"https://api.cloudflare.com/client/v4/zones?name={domain}");
                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);

                if (json["success"].Value<bool>() && json["result"].HasValues)
                {
                    zoneId = json["result"][0]["id"].Value<string>();
                    logger.Log($"Retrieved zone ID for domain {domain}: {zoneId}");
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

        // Mantengo la vecchia firma per compatibilità con il codice esistente
        public async Task<string> GetZoneIdAsync()
        {
            if (!string.IsNullOrEmpty(zoneId))
                return zoneId;

            return await GetZoneIdInternalAsync();
        }

        public (bool Success, string Message) UpdateDnsRecords(string newIp)
        {
            try
            {
                // Cancella i record aggiornati dalla precedente esecuzione
                updatedRecords.Clear();

                // Usa il nuovo metodo sincrono
                var zoneId = GetZoneId();
                if (string.IsNullOrEmpty(zoneId))
                    return (false, "Failed to get zone ID");

                logger.Log($"Checking DNS records for domain {domain} (Zone ID: {zoneId})");

                // Esegui le chiamate HTTP in modo sicuro
                return Task.Run(async () => await UpdateDnsRecordsInternalAsync(zoneId, newIp)).Result;
            }
            catch (Exception ex)
            {
                logger.Log($"Error updating DNS records: {ex.Message}");
                return (false, ex.Message);
            }
        }

        private async Task<(bool Success, string Message)> UpdateDnsRecordsInternalAsync(string zoneId, string newIp)
        {
            try
            {
                // Get all A records
                var recordsResponse = await client.GetAsync($"https://api.cloudflare.com/client/v4/zones/{zoneId}/dns_records?type=A");
                var recordsContent = await recordsResponse.Content.ReadAsStringAsync();
                var recordsJson = JObject.Parse(recordsContent);

                if (!recordsJson["success"].Value<bool>())
                    return (false, $"Failed to get DNS records: {recordsContent}");

                var records = recordsJson["result"].ToObject<JArray>();
                logger.Log($"Found {records.Count} DNS A records to check");
                bool anyUpdated = false;

                foreach (var record in records)
                {
                    string recordId = record["id"].Value<string>();
                    string currentIp = record["content"].Value<string>();
                    string recordName = record["name"].Value<string>();

                    var recordUpdate = new DnsRecordUpdate
                    {
                        Name = recordName,
                        OldIp = currentIp,
                        NewIp = newIp,
                        Status = "Checked"
                    };

                    if (currentIp == newIp)
                    {
                        recordUpdate.Status = "No change needed";
                        updatedRecords.Add(recordUpdate);
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
                    // Imposta il Content-Type direttamente sull'oggetto StringContent
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var updateResponse = await client.PutAsync($"https://api.cloudflare.com/client/v4/zones/{zoneId}/dns_records/{recordId}", content);
                    var updateContent = await updateResponse.Content.ReadAsStringAsync();
                    var updateJson = JObject.Parse(updateContent);

                    if (updateJson["success"].Value<bool>())
                    {
                        recordUpdate.Status = "Updated";
                        anyUpdated = true;
                    }
                    else
                    {
                        recordUpdate.Status = $"Failed: {updateJson["errors"]?.ToString() ?? "Unknown error"}";
                    }

                    updatedRecords.Add(recordUpdate);
                }

                if (anyUpdated)
                    return (true, $"Updated {updatedRecords.Count(r => r.Status == "Updated")} DNS records successfully");
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

