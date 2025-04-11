
using System;
using System.Threading;
using System.Threading.Tasks;





class Program
{
    static async Task Output20RandomNumbers()
    {
        await Task.Run(() =>
        {
            Random random = new Random();
            List<int> numbers = new List<int>();
            
            Console.Write("Task1 - ");
            for (int i = 0; i < 20; i++)
            {
                numbers.Add(random.Next(1, 101));
                Console.Write($"{numbers[i]} ");
            }
            Console.WriteLine();
        });
    }
    
    static async Task<List<int>> FilterEvenAsync(List<int> list)
    {
        await Task.Delay(1500);
        List<int> numbers = list.Where(x => x % 2 == 0).ToList();
        
        Console.Write("Task2 - ");
        foreach (int i in numbers)
        {
            Console.Write($"{i} ");
        }
        Console.WriteLine();
        return numbers;
    }
    
    static async Task<List<int>> FilterOddAsync(List<int> list)
    {
        await Task.Delay(1500);
        List<int> numbers = list.Where(x => x % 2 != 0).ToList();
        
        Console.Write("Task3 - ");
        foreach (int i in numbers)
        {
            Console.Write($"{i} ");
        }
        Console.WriteLine();
        return numbers;
    }


    static async Task<int> CalculateSumAsync(List<int> numbers)
    {
        await Task.Delay(1000);
        Console.WriteLine($"Task4 - {numbers.Sum()}");
        return numbers.Sum();
    }
    
    
    static async Task Main(string[] args)
    {
        Console.WriteLine("Main начал работу");
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        // 1----------------------------
        
         Task task1 = Output20RandomNumbers();
         await task1;
         
         //2----------------------------------------
        
         List<int> NewNumbers1 = await FilterEvenAsync(numbers);
         
         //3----------------------------------------------------
         
         List<int> NewNumbers2 = await FilterOddAsync(numbers);
         
         //4----------------------------------------------------
         
         int sum = await CalculateSumAsync(numbers);

         
        Console.WriteLine("Main завершён");
    }
}

    
    
    