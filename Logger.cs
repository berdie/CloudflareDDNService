using System;
using System.IO;
using System.Text;

namespace CloudflareDDNService
{
    public class Logger
    {
        private readonly string logDirectory;
        private readonly string logFilePath;
        private readonly int maxLogSizeBytes = 5 * 1024 * 1024; // 5 MB
        private readonly int maxLogFiles = 5;

        public Logger()
        {
            logDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "CloudflareDDNService",
                "Logs");
            
            Directory.CreateDirectory(logDirectory);
            logFilePath = Path.Combine(logDirectory, "service.log");
        }

        public void Log(string message)
        {
            try
            {
                CheckLogFileSize();
                
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                
                Console.WriteLine(logEntry); // Also output to console for debugging
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log: {ex.Message}");
            }
        }

        private void CheckLogFileSize()
        {
            if (!File.Exists(logFilePath))
                return;
                
            var fileInfo = new FileInfo(logFilePath);
            if (fileInfo.Length >= maxLogSizeBytes)
            {
                RotateLogs();
            }
        }

        private void RotateLogs()
        {
            // Delete oldest log file if we have reached the maximum
            string oldestLog = Path.Combine(logDirectory, $"service.{maxLogFiles}.log");
            if (File.Exists(oldestLog))
            {
                File.Delete(oldestLog);
            }

            // Shift all other log files
            for (int i = maxLogFiles - 1; i >= 1; i--)
            {
                string currentLog = Path.Combine(logDirectory, $"service.{i}.log");
                string nextLog = Path.Combine(logDirectory, $"service.{i + 1}.log");
                
                if (File.Exists(currentLog))
                {
                    File.Move(currentLog, nextLog);
                }
            }

            // Move current log to service.1.log
            File.Move(logFilePath, Path.Combine(logDirectory, "service.1.log"));
        }
    }
}
