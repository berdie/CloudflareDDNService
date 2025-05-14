using System;
using System.IO;
using Newtonsoft.Json;

namespace CloudflareDDNService
{
    public class Configuration
    {
        public string ApiKey { get; set; }
        public string Email { get; set; }
        public string Domain { get; set; }
        public int UpdateInterval { get; set; } = 30; // Default 30 minutes
    }

    public class ConfigurationManager
    {
        private readonly string configPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "CloudflareDDNService",
            "config.json");
        private readonly Logger logger;

        public ConfigurationManager()
        {
            logger = new Logger();
            
            // Ensure directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(configPath));
        }

        public Configuration LoadConfiguration()
        {
            try
            {
                if (File.Exists(configPath))
                {
                    var json = File.ReadAllText(configPath);
                    return JsonConvert.DeserializeObject<Configuration>(json);
                }
            }
            catch (Exception ex)
            {
                logger.Log($"Error loading configuration: {ex.Message}");
            }

            // Return default configuration if loading fails
            return new Configuration();
        }

        public void SaveConfiguration(Configuration config)
        {
            try
            {
                var json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(configPath, json);
                logger.Log("Configuration saved successfully");
            }
            catch (Exception ex)
            {
                logger.Log($"Error saving configuration: {ex.Message}");
            }
        }
    }
}
