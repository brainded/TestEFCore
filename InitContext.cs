using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace TestingEFCoreBehavior
{
    public class InitContext : IDisposable
    {
        private TestContext _testContext;
        private SqlConnection _connection;
        private string _connectionString;

        private static int _openConnections;

        public InitContext(string connectionString) 
        {
            _connectionString = connectionString;
            Init();
        }

        public void Init()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
            _connection.Disposed += delegate
            {
                _openConnections--;
                Trace.WriteLine($"InitContext:CLOSE:OPENCONNECTIONS:{_openConnections}");
            };

            _openConnections++;
            Trace.WriteLine($"InitContext:OPEN:OPENCONNECTIONS:{_openConnections}");

            var options = new DbContextOptionsBuilder<TestContext>();
            options
                .UseSqlServer(_connection, x =>
                {

                });

            _testContext = new TestContext(options.Options);
        }

        public List<Item> GetItems()
        {
            return _testContext.Items.ToList();
        }

        public void Dispose()
        {
            
        }
    }
}
