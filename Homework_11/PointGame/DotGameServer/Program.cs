namespace TcpSeminarOris;

public static class Program
{
    public static async Task Main(string[] args)
    {
        ServerObject server = new ServerObject();// создаем сервер
        await server.ListenAsync(); // запускаем сервер
    }
}