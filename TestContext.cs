using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;

namespace TestingEFCoreBehavior
{
    public class TestContext : DbContext
    {
        public TestContext() { }

        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=192.168.0.244,1533;Database=testefcore;User ID=sa;Password=97dc6b81-bfbd-418a-aecb-3763f393af00;Persist Security Info=False;TrustServerCertificate=true;");
        //}
    }
}
