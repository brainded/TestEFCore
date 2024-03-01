using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace TestingEFCoreBehavior
{
    public enum ConnectionCleanup
    {
        Close,
        Dispose
    }

    public class InitContext : IDisposable
    {
        private TestContext _testContext;
        private SqlConnection _connection;
        private string _connectionString;
        private ConnectionCleanup _cleanup;

        public InitContext(string connectionString, ConnectionCleanup connectionCleanup) 
        {
            _connectionString = connectionString;
            _cleanup = connectionCleanup;
            Init();
        }

        public void Init()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var options = new DbContextOptionsBuilder<TestContext>();
            options.UseSqlServer(_connection);

            _testContext = new TestContext(options.Options);
        }

        public List<Item> GetItems()
        {
            return _testContext.Items.ToList();
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                switch (_cleanup)
                {
                    case ConnectionCleanup.Close:
                        _connection.Close();
                        break;
                    case ConnectionCleanup.Dispose:
                        _connection.Dispose();
                        break;
                }
            }

            if (_testContext != null)
            {
                _testContext.Dispose();
            }
        }
    }
}
