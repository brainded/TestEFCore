using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;

namespace TestingEFCoreBehavior
{
    public class TestContext : DbContext
    {
        //private readonly string _connectionString;

        //public TestContext(string connectionString) 
        //{ 
        //    _connectionString = connectionString;
        //}

        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        {

        }

        public DbSet<Item> Items { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(_connectionString);
        //}
    }
}
