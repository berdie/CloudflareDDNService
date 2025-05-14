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
            try
            {
                logger = new Logger();
                
                // Ensure directory exists
                string configDir = Path.GetDirectoryName(configPath);
                if (!Directory.Exists(configDir))
                {
                    Directory.CreateDirectory(configDir);
                }
            }
            catch (Exception ex)
            {
                // In caso di errore, utilizza una posizione alternativa
                string appDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                configPath = Path.Combine(appDir, "config.json");
                
                try
                {
                    logger = new Logger();
                    logger.Log($"Error creating configuration directory, using alternate path: {configPath}");
                    logger.Log($"Error details: {ex.Message}");
                }
                catch
                {
                    // Se anche la creazione del logger fallisce, possiamo solo continuare silenziosamente
                }
            }
        }

        public Configuration LoadConfiguration()
        {
            try
            {
                if (File.Exists(configPath))
                {
                    var json = File.ReadAllText(configPath);
                    var config = JsonConvert.DeserializeObject<Configuration>(json);
                    
                    // Verifica che la deserializzazione abbia funzionato
                    if (config == null)
                    {
                        logger.Log("Configuration file exists but could not be deserialized");
                        return new Configuration();
                    }
                    
                    return config;
                }
            }
            catch (Exception ex)
            {
                if (logger != null)
                {
                    logger.Log($"Error loading configuration: {ex.Message}");
                }
                
                // Prova percorso alternativo
                try
                {
                    string appDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string alternatePath = Path.Combine(appDir, "config.json");
                    
                    if (File.Exists(alternatePath))
                    {
                        var json = File.ReadAllText(alternatePath);
                        return JsonConvert.DeserializeObject<Configuration>(json) ?? new Configuration();
                    }
                }
                catch
                {
                    // Ignora gli errori sul percorso alternativo e torna alla configurazione predefinita
                }
            }

            // Return default configuration if loading fails
            return new Configuration();
        }

        public void SaveConfiguration(Configuration config)
        {
            try
            {
                var json = JsonConvert.SerializeObject(config, Formatting.Indented);
                
                // Assicurati che la directory esista prima del salvataggio
                string configDir = Path.GetDirectoryName(configPath);
                if (!Directory.Exists(configDir))
                {
                    Directory.CreateDirectory(configDir);
                }
                
                File.WriteAllText(configPath, json);
                logger.Log("Configuration saved successfully");
            }
            catch (Exception ex)
            {
                logger.Log($"Error saving configuration: {ex.Message}");
                
                // Prova a salvare nella directory dell'applicazione
                try 
                {
                    string appDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string alternatePath = Path.Combine(appDir, "config.json");
                    
                    var json = JsonConvert.SerializeObject(config, Formatting.Indented);
                    File.WriteAllText(alternatePath, json);
                    logger.Log($"Configuration saved to alternate location: {alternatePath}");
                }
                catch (Exception altEx)
                {
                    logger.Log($"Failed to save configuration to alternate location: {altEx.Message}");
                }
            }
        }
    }
}
