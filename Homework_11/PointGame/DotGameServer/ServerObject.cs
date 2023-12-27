using GameModels;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.Json;

namespace TcpSeminarOris;


class ServerObject
{
    TcpListener tcpListener = new TcpListener(IPAddress.Any, 8888);
    public List<ClientObject> clients = new List<ClientObject>();

    public List<DrawingPoint> Points = new List<DrawingPoint>();

    protected internal void RemoveConnection(string id)
    {
        ClientObject? client = clients.FirstOrDefault(c => c.Id == id);

        if (client != null) clients.Remove(client);
        client?.Close();
    }

    protected internal async Task ListenAsync()
    {
        try
        {
            tcpListener.Start();
            Console.WriteLine("Сервер запущен.");

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                ClientObject clientObject = new ClientObject(tcpClient, this);
                clients.Add(clientObject);
                Task.Run(clientObject.ProcessAsync);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    protected internal async Task BroadcastMessageAsync()
    {
        var response = GetServerResponse();

        foreach (var client in clients)
        {
            await client.Writer.WriteLineAsync(JsonSerializer.Serialize(response));
            await client.Writer.FlushAsync();
            await Console.Out.WriteLineAsync("Результат отправлен");
        }
    }

    private ServerResponse GetServerResponse() =>
         new ServerResponse() { Users = GetAllUsers(), DrawingPoints = GetDrawingMap() };


    private DrawingPoint[] GetDrawingMap() =>
        clients.SelectMany(x => x.DrawingPoints).ToArray();


    public User[] GetAllUsers() =>
         clients.Select(x => x.CurrentUser).ToArray();

}


class ClientObject
{
    protected internal string Id { get; } = Guid.NewGuid().ToString();
    protected internal StreamWriter Writer { get; }
    protected internal StreamReader Reader { get; }

    public User CurrentUser;
    public List<DrawingPoint> DrawingPoints;

    TcpClient client;
    ServerObject Server;

    public ClientObject(TcpClient tcpClient, ServerObject ServerObject)
    {
        client = tcpClient;
        Server = ServerObject;
        var stream = client.GetStream();
        DrawingPoints = new List<DrawingPoint>();

        Reader = new StreamReader(stream);
        Writer = new StreamWriter(stream);
    }

    public async Task ProcessAsync()
    {
        try
        {
            var message = await Reader.ReadLineAsync();
            if (string.IsNullOrEmpty(message)) return;
            var model = JsonSerializer.Deserialize<User>(message);


            CurrentUser = model;
            await Console.Out.WriteLineAsync($"Новый пользователь: {model.Name}");

            await Server.BroadcastMessageAsync();


            while (true)
            {
                try
                {
                    var m = await Reader.ReadLineAsync();

                    if (string.IsNullOrEmpty(m)) continue;

                    var map = JsonSerializer.Deserialize<DrawingPoint>(m);

                    DrawingPoints.Add(map);

                    await Server.BroadcastMessageAsync();
                }
                catch
                {
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            Server.RemoveConnection(Id);
        }
    }
    protected internal void Close()
    {
        Writer.Close();
        Reader.Close();
        client.Close();
    }
}