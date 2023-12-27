using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp_HTTP.Configuration;

namespace ConsoleApp_HTTP
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var config = ServerConfiguration.Load("appsettings.json");

                // Создание папки static, если её нет
                Directory.CreateDirectory(config.StaticFilesPath);

                var server = new HttpServer(config);

                Console.WriteLine("Запуск сервера");
                server.Start(); // Начинаем прослушивать входящие подключения
                Console.WriteLine("Сервер запущен!");

                // Ожидание завершения работы сервера (можно добавить логику остановки)
                Console.WriteLine("Для остановки сервера нажмите любую клавишу...");
                Console.ReadKey();
                server.Stop();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл не найден");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Сервер завершил свою работу");
            }
        }
    }
}