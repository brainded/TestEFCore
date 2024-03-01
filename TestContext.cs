using Microsoft.EntityFrameworkCore;

namespace TestingEFCoreBehavior
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        {

        }

        public DbSet<Item> Items { get; set; }
    }
}
