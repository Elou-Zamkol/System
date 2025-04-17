﻿using System.Net;
using System.Net.Sockets;

var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

var address = IPAddress.Parse("172.20.28.184");
var serverEndPoint = new IPEndPoint(address, 3003);

try
{
    clientSocket.Connect(serverEndPoint);
    
    while (true)
    {
        Console.WriteLine("Enter message to send to server or type 'quit' to exit:");
        string message = Console.ReadLine();

        byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
        clientSocket.Send(messageBytes);
        Console.WriteLine($"Sent message: {message}");
        
        byte[] Buffer = new byte[1024];
        int BytesRead = clientSocket.Receive(Buffer);
        string response = System.Text.Encoding.UTF8.GetString(Buffer, 0, BytesRead);
        Console.WriteLine($"Server responded: {response}");

        if (message.ToLower() == "quit")
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            break;
        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
}

    
    
    