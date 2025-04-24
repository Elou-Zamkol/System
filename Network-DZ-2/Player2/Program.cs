using System.Net;
using System.Net.Sockets;
using System.Text;


static bool SiteCheck(int highlightX, int highlightY, string masiv)
{
    for (int y = 0; y < 3; y++)
    {
        for (int x = 0; x < 3; x++)
        {
            if (masiv[x + (y * 3)] != ' ' && x == highlightX && y == highlightY)
            {
                return false;
                
            }
        }
    }
    return true;
}



static void DrawField(int highlightX, int highlightY, string masiv)
{
    for (int y = 0; y < 3; y++)
    {
        for (int x = 0; x < 3; x++)
        {
            if (masiv[x + (y * 3)] != ' ')
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"[{masiv[x + (y * 3)]}]");
                Console.ResetColor();
            }
            
            else if (x == highlightX && y == highlightY)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("[ ]");
                Console.ResetColor();
            }
            else
            {
                Console.Write("[ ]");
            }
        }
        Console.WriteLine();
    }
}

static void EndWin(string masiv, string end)
{
    int count = 0;
    for (int y = 0; y < 3; y++)
    {
        for (int x = 0; x < 3; x++)
        {

            if (Convert.ToInt32(Convert.ToString(end[0 + count])) == y && Convert.ToInt32(Convert.ToString(end[1 + count])) == x)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"[{masiv[x + (y * 3)]}]");
                Console.ResetColor();
                if(count < 4) count += 2;
            }
             
            else if (masiv[x + (y * 3)] != ' ')
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"[{masiv[x + (y * 3)]}]");
                Console.ResetColor();
            }
            
            else
            {
                Console.Write("[ ]");
            }
        }
        Console.WriteLine();
    }
}


static async Task<string> Expectation(StreamReader reader)
{
    Console.Write("Wait for player's turn!!!");
    string? masiv = await reader.ReadLineAsync();
    Console.Clear();
    return masiv;
}

//---------------------------------------------------------------




using var client = new TcpClient();
await client.ConnectAsync("172.17.0.1", 3003);

using var networkStream = client.GetStream();
using var writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true };
using var reader = new StreamReader(networkStream, Encoding.UTF8);

int cursorX = 0;
int cursorY = 0;

bool FlagEnd = true;

while (true)
{
    bool flag = true;
    
    Console.CursorVisible = false;
    string masiv = await Expectation(reader);
    Console.CursorVisible = true;
    
    string end = await reader.ReadLineAsync();
    
    Console.WriteLine($"l- {end.Length}");

    if (end.Length == 6)
    { 
        Console.WriteLine($"m- {masiv}");
        Console.WriteLine($"m- {masiv.Length}");
        char Simbol = masiv[Convert.ToInt32(Convert.ToString(end[0])) + (Convert.ToInt32(Convert.ToString(end[1])) * 3)];
        
        if (end == "Draw")
        {
            Console.Clear();
            Console.WriteLine("---------Draw-----------");
            FlagEnd = false;
        }
    
        else if (Simbol == 'O')
        {
            Console.Clear();
            EndWin(masiv, end);
            Console.WriteLine("---------You Win-----------");
            FlagEnd = false;
        }
    
        else if (Simbol == 'X')
        {
            Console.Clear();
            Console.WriteLine("---------You lost-----------");
            FlagEnd = false;
        }
    } 

    
    
    
    Console.CursorVisible = false;

    while (flag && FlagEnd)
    {
        Console.Clear();
        DrawField(cursorX, cursorY, masiv);

        ConsoleKeyInfo key = Console.ReadKey(true);

        switch (key.Key)
        {
            case ConsoleKey.LeftArrow:
                if (cursorX > 0) cursorX--;
                break;
            case ConsoleKey.RightArrow:
                if (cursorX < 3 - 1) cursorX++;
                break;
            case ConsoleKey.UpArrow:
                if (cursorY > 0) cursorY--;
                break;
            case ConsoleKey.DownArrow:
                if (cursorY < 3 - 1) cursorY++;
                break;
            case ConsoleKey.Enter:
                
                if (SiteCheck(cursorX, cursorY, masiv))
                {
                    Console.Clear();
                    Console.WriteLine($"Вы выбрали ячейку: ({cursorX}, {cursorY})");
                    flag = false;
                }
                
                break;
        }
    }
    
    string message = string.Join(' ', cursorX, cursorY);
    await writer.WriteLineAsync(message);
    await writer.WriteLineAsync("O");

    if (!FlagEnd) break;
    
}


