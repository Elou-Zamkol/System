using System.Net;
using System.Net.Sockets;
using System.Text;

static string CheckWinnerOrGameStatus(string[,] game)
{

    for (int i = 0; i < 3; i++)
    {
        
        if (game[i, 0] == game[i, 1] && game[i, 0] == game[i, 2] && game[i, 0] != null)
        {
            return $"{i}0{i}1{i}2";
        }
        
        if (game[0, i] == game[1, i] && game[0, i] == game[2, i] && game[0, i] != null)
        {
            return $"0{i}1{i}2{i}";
        }
        
        if (game[0, 0] == game[1, 1] && game[0, 0] == game[2, 2] && game[0, 0] != null)
        {
            return $"001122";
        }
    
        if (game[0, 2] == game[1, 1] && game[0, 2] == game[2, 0] && game[0, 2] != null)
        {
            return $"021120";
        }
    }
    
    bool BoardFull = true;
    for (int y = 0; y < 3; y++)
    {
        for (int x = 0; x < 3; x++)
        {
            if (game[y, x] == null || game[y, x] == " ")
            {
                BoardFull = false;
                break;
            }
        }
    }

    if (BoardFull)
    {
        return "Draw";
    }
    
    return "";
    

}

TcpListener listener = new(IPAddress.Any, 3003);
listener.Start();
Console.WriteLine("Server started. Waiting for connections...");

TcpClient client1 = await listener.AcceptTcpClientAsync();
Console.WriteLine("Client 1 connected!");

TcpClient client2 = await listener.AcceptTcpClientAsync();
Console.WriteLine("Client 2 connected!");

ManualResetEvent flag1 = new(false);
ManualResetEvent flag2 = new(false);

string[,] game = new string[3, 3];
string end = "";

Task RunClient(TcpClient client, ManualResetEvent myTurn, ManualResetEvent otherTurn)
{
    return Task.Run(async () =>
    {
        using NetworkStream stream = client.GetStream();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

        while (true)
        {
            myTurn.WaitOne();

            string masiv = "";

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    masiv += game[x, y] ?? " ";
                }
            }
            
            await writer.WriteLineAsync(masiv);
            
            end = CheckWinnerOrGameStatus(game);
            
            await writer.WriteLineAsync(end);

            string? message = await reader.ReadLineAsync();
            string? symbol = await reader.ReadLineAsync();

            game[Convert.ToInt32(Convert.ToString(message[2])), Convert.ToInt32(Convert.ToString(message[0]))] = symbol;
            
            myTurn.Reset();
            otherTurn.Set();
            
            if(end.Length == 6) break;
        }
    });
}

Task task1 = RunClient(client1, flag1, flag2);
Task task2 = RunClient(client2, flag2, flag1);

flag1.Set();

await Task.WhenAll(task1, task2);


