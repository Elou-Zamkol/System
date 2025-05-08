using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.Interactivity;
using MyProgekt;


namespace TcpChatAvalonia
{
    public partial class MainWindow : Window
    {
        private ChatServer _server;
        private ChatClient _client;
        private ObservableCollection<string> _messages = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            MessagesListBox.ItemsSource = _messages;
            IpTextBox.Text = GetLocalIPAddress();
            PortTextBox.Text = "5000";
        }

        private void StartButton_Click(object? sender, RoutedEventArgs e)
        {
            string ip = IpTextBox.Text;
            int remotePort = int.Parse(PortTextBox.Text);
            int myPort = int.Parse(MyPortTextBox.Text);

            _server = new ChatServer(myPort);
            _server.MessageReceived += OnMessageReceived;
            _server.Start();

            _client = new ChatClient(ip, remotePort);
            _client.Start();

            AddMessage("[Система] Чат запущен.");
        }

        private void SendButton_Click(object? sender, RoutedEventArgs e)
        {
            string text = MessageTextBox.Text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                _client.SendMessage(text);
                AddMessage("[Вы]: " + text);
                MessageTextBox.Text = string.Empty;
            }
        }

        private void OnMessageReceived(string message)
        {
            Dispatcher.UIThread.Post(() =>
            {
                AddMessage("[Собеседник]: " + message);
            });
        }

        private void AddMessage(string text)
        {
            _messages.Add(text);
        }
        
        private string GetLocalIPAddress() // Эльвин Муалим если что нахождения вашего IP я посмотрел в интернете это чисто для удобство :)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }
        
    }
}