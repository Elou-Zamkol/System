using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyProgekt;

public class ChatServer
{
    public event Action<string> MessageReceived;

    private int _port;

    public ChatServer(int port)
    {
        _port = port;
    }

    public void Start()
    {
        Task.Run(() =>
        {
            var listener = new TcpListener(IPAddress.Any, _port);
            listener.Start();

            while (true)
            {
                var client = listener.AcceptTcpClient();
                _ = HandleClient(client);
            }
        });
    }

    private async Task HandleClient(TcpClient client)
    {
        using var stream = client.GetStream();
        byte[] buffer = new byte[1024];

        while (true)
        {
            int count = await stream.ReadAsync(buffer, 0, buffer.Length);
            if (count == 0) break;

            string msg = Encoding.UTF8.GetString(buffer, 0, count);
            MessageReceived?.Invoke(msg);
        }
    }
}