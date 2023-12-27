using System;
using System.IO;
using System.Text.Json;

namespace ConsoleApp_HTTP.Configuration
{
    public class ServerConfiguration
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public string StaticFilesPath { get; set; }

        public static ServerConfiguration Load(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<ServerConfiguration>(json);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при загрузке конфигурации сервера.", ex);
            }
        }
    }
}