using System.Net;
using System.Net.Sockets;

var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

var address = IPAddress.Parse("172.20.28.184");

var endPoint = new IPEndPoint(address, 3003);



try
{
    serverSocket.Bind(endPoint);
    serverSocket.Listen();
    Console.WriteLine($"Listening on {endPoint.Address}:{endPoint.Port}");
    while (true)
    {
        var buffer = new byte[1024];
        var clientSocket = serverSocket.Accept();
        Console.WriteLine($"Client connected: {clientSocket.RemoteEndPoint}");
        
        ThreadPool.QueueUserWorkItem(state =>
        {
            
            while (true)
            {
                var bytesRead = clientSocket.Receive(buffer);
                var message = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received message: {message}");
        
                string ResponseMessage = $"Server received: {message}";
                var ResponseBytes = System.Text.Encoding.UTF8.GetBytes(ResponseMessage);
                clientSocket.Send(ResponseBytes);
        
       
                if (message.ToLower() == "quit")
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    break;
                }
            }
            
        }, clientSocket);
        
        
    }
    
}
catch (Exception e)
{
    Console.WriteLine(e);
}