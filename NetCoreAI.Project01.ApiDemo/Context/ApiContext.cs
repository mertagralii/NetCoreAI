using Microsoft.EntityFrameworkCore;
using NetCoreAI.Project01.ApiDemo.Entities;

namespace NetCoreAI.Project01.ApiDemo.Context
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=MERT;Database=ApiAIDb;Integrated Security=true;TrustServerCertificate=True");
        }
        public DbSet<Customer> Customers { get; set; }
    }
}
