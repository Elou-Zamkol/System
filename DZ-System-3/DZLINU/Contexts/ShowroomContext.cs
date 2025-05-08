using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DZ.Contexts;

public class ShowroomContext : DbContext
{
    
    public DbSet<Student> Students { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()
            .GetConnectionString("Default");
        optionsBuilder.UseSqlServer(configurationBuilder);
        
    }
    
    
}






    
