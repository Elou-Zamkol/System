using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DZ.Contexts;
using Microsoft.EntityFrameworkCore;


class Program
{
    static void Main(string[] args)
    {
        var context = new ShowroomContext();
        context.Database.EnsureCreated();

        AddStudentManually("Raul");
        ShowAllStudentsManually();

        AddStudentAsync("Elvin").GetAwaiter().GetResult();
        ShowAllStudentsAsync().GetAwaiter().GetResult();
    }
    
    static void AddStudentManually(string name)
    {
        Console.WriteLine($"Manual add - Thread ID: {Thread.CurrentThread.ManagedThreadId}");
        Task.Run(() =>
        {
            var context = new ShowroomContext();
            context.Students.Add(new Student { Name = name });
            context.SaveChanges();
            Console.WriteLine($"Manual add in Task - Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            
        }).GetAwaiter().GetResult();
    }

    static void ShowAllStudentsManually()
    {
        Console.WriteLine($"Manual show - Thread ID: {Thread.CurrentThread.ManagedThreadId}");
        var students = Task.Run(() =>
        {
            var context = new ShowroomContext();
            Console.WriteLine($"Manual show in Task - Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            return context.Students.ToList();
            
        }).GetAwaiter().GetResult();

        foreach (var student in students)
        {
            Console.WriteLine($"Student: {student.Id}, {student.Name}");
        }
    }

    static async Task AddStudentAsync(string name)
    {
        Console.WriteLine($"Async add - Thread ID: {Thread.CurrentThread.ManagedThreadId}");
        var context = new ShowroomContext();
        
        context.Students.Add(new Student { Name = name });
        await context.SaveChangesAsync();
        Console.WriteLine($"Async add after await - Thread ID: {Thread.CurrentThread.ManagedThreadId}");
        
    }

    static async Task ShowAllStudentsAsync()
    {
        Console.WriteLine($"Async show - Thread ID: {Thread.CurrentThread.ManagedThreadId}");
        var context = new ShowroomContext();
        
        var students = await context.Students.ToListAsync();
        Console.WriteLine($"Async show after await - Thread ID: {Thread.CurrentThread.ManagedThreadId}");
        foreach (var student in students)
        {
            Console.WriteLine($"Student: {student.Id}, {student.Name}");
        }
        
    }
}
