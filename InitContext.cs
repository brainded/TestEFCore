using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace TestingEFCoreBehavior
{
    public enum ConnectionCleanup
    {
        None,
        Close,
        Dispose
    }

    public class InitContext : IDisposable
    {

        private ShardManager _shardManager;
        private readonly ITenant _tenant;
        private ConnectionCleanup _cleanup;

        private TestContext _testContext;
        private SqlConnection _connection;

        public InitContext(ShardManager shardManager, ITenant tenant) 
        {
            _shardManager = shardManager;
            _tenant = tenant;
            _cleanup = ConnectionCleanup.Close;
            
            Init();
        }

        public void Init()
        {
            _connection = _shardManager.OpenConnectionForKey(_tenant.TenantId);

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
