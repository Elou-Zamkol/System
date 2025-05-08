using System.Net.Sockets;
using System.Text;

namespace MyProgekt;

public class ChatClient
{
    private string _ip;
    private int _port;
    private TcpClient _client;
    private NetworkStream _stream;

    public ChatClient(string ip, int port)
    {
        _ip = ip;
        _port = port;
    }

    public async void Start()
    {
        _client = new TcpClient();
        await _client.ConnectAsync(_ip, _port);
        _stream = _client.GetStream();
    }

    public async void SendMessage(string text)
    {
        if (_stream != null)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            await _stream.WriteAsync(data, 0, data.Length);
        }
    }
}