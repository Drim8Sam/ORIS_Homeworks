using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp_HTTP.Configuration;

namespace ConsoleApp_HTTP
{
    public class HttpServer
    {
        private readonly ServerConfiguration _config;
        private readonly HttpListener _server;

        public HttpServer(ServerConfiguration config)
        {
            _config = config;
            _server = new HttpListener();
            _server.Prefixes.Add($"http://{_config.Address}:{_config.Port}/");
        }

        public void Start()
        {
            _server.Start();
            Task.Run(ProcessRequests);
        }

        public void Stop()
        {
            _server.Stop();
        }

        private async Task ProcessRequests()
        {
            while (_server.IsListening)
            {
                try
                {
                    var context = await _server.GetContextAsync();

                    var uri = context.Request.Url;

                    string path = Path.Combine(Directory.GetCurrentDirectory(), uri.AbsolutePath.TrimStart('/'));

                    if (string.IsNullOrEmpty(Path.GetExtension(path)))
                    {
                        if (uri.AbsolutePath.EndsWith("/"))
                        {
                            path = Path.Combine(path, "index.html");
                        }
                        
                        Console.WriteLine($"Запрос к неразрешенному файлу: {path}");
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        using (var writer = new StreamWriter(context.Response.OutputStream))
                        {
                            await writer.WriteAsync("400 - Bad Request");
                        }
                        continue;
                    }

                    if (!File.Exists(path))
                    {
                        Console.WriteLine($"Файл {path} не найден");
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        using (var writer = new StreamWriter(context.Response.OutputStream))
                        {
                            await writer.WriteAsync("404 - File Not Found");
                        }
                        continue;
                    }

                    var response = context.Response;

                    byte[] buffer = File.ReadAllBytes(path);

                    response.ContentLength64 = buffer.Length;
                    using Stream output = response.OutputStream;

                    await output.WriteAsync(buffer);
                    await output.FlushAsync();

                    Console.WriteLine("Запрос обработан");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при обработке запроса: {ex.Message}");
                }
            }
        }
    }
}
