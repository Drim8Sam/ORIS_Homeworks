using GameModels;
using Microsoft.VisualBasic.ApplicationServices;
using System.Net.Sockets;
using System.Text.Json;

namespace DotGame;

public partial class GameWindow : Form
{
    public StreamWriter Writer { get; set; }
    public StreamReader Reader { get; set; }

    public Graphics G { get; set; }

    public GameWindow()
    {
        InitializeComponent();
        G = CreateGraphics();
    }

    private async void Login(object sender, EventArgs e)
    {
        string host = "127.0.0.1";
        int port = 8888;
        using TcpClient client = new TcpClient();
        string? userName = NameInput.Text;

        try
        {
            client.Connect(host, port);
            Reader = new StreamReader(client.GetStream());
            Writer = new StreamWriter(client.GetStream());

            if (Writer is null || Reader is null) return;
            await SendNameAsync(Writer, userName);

            ButtonLogin.Enabled = false;
            ButtonLogin.Visible = false;
            NameInput.Visible = false;
            NameInput.Enabled = false;
            await ReceiveMessageAsync(Reader);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    private async Task ReceiveMessageAsync(StreamReader reader)
    {
        while (true)
        {
            try
            {
                var response = await reader.ReadLineAsync();
                if (string.IsNullOrEmpty(response)) continue;

                var serverMessage = JsonSerializer.Deserialize<ServerResponse>(response);
                if (serverMessage == null) continue;
                Users.Items.Clear();
                Users.Items.AddRange(serverMessage.Users
                    .Select(x => x.Name)
                    .ToArray());

                DrawPoints(serverMessage.DrawingPoints);

            }
            catch
            {
                break;
            }
        }
    }

    private void DrawPoints(DrawingPoint[] drawingPoints)
    {
        if (drawingPoints.Length == 0) return;
        foreach (var point in drawingPoints)
            DrawPoint(point.X, point.Y, point.Width, point.Color);
    }

    private void DrawPoint(int x, int y, int size, Color color)
    {
        var rect = new Rectangle(x, y, size, size);
        var pen = new Pen(Color.Blue);
        var brush = new SolidBrush(Color.Blue);
        G.DrawEllipse(pen, rect);
        G.FillEllipse(brush, rect);
    }

    private async Task SendNameAsync(StreamWriter writer, string userName)
    {
        var user = new GameModels.User { Name = userName, Color = Color.FromArgb(100, 123, 123, 123) };
        var json = JsonSerializer.Serialize(user);
        await writer.WriteLineAsync(json);
        await writer.FlushAsync();
    }

    private async void GameWindow_MouseDown(object sender, MouseEventArgs e)
    {
        await SendPointAsync(e.X, e.Y, 10, Color.Blue);
    }

    private async Task SendPointAsync(int x, int y, int size, Color color)
    {
        var point = new DrawingPoint { X = x, Y = y, Color = color, Width = size };
        var json = JsonSerializer.Serialize(point);
        await Writer.WriteLineAsync(json);
        await Writer.FlushAsync();
    }
}