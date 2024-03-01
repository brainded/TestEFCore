using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using Microsoft.Data.SqlClient;

namespace TestingEFCoreBehavior
{
    public class ShardConfiguration
    {
        public string ConnectionString { get; set; }

        public string Server { get; set; }

        public string Database { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class ShardManager
    {
        private const string ShardMapName = "Test";

        private readonly ShardConfiguration _shardConfiguration;

        public ShardMapManager _shardMapManager;

        public ListShardMap<int> _shardMap;

        public ShardManager(ShardConfiguration shardConfiguration) 
        {
            _shardConfiguration = shardConfiguration;

            //get or init the shard map manager
            if (!ShardMapManagerFactory.TryGetSqlShardMapManager(_shardConfiguration.ConnectionString, ShardMapManagerLoadPolicy.Lazy, out ShardMapManager shardMapManager))
            {
                _shardMapManager = ShardMapManagerFactory.CreateSqlShardMapManager(_shardConfiguration.ConnectionString);
            }
            else
            {
                _shardMapManager = shardMapManager;
            }

            //get or init the shard map
            if (!_shardMapManager.TryGetListShardMap<int>(ShardMapName, out ListShardMap<int> shardMap))
            {
                _shardMap = _shardMapManager.CreateListShardMap<int>(ShardMapName);
            }
            else
            {
                _shardMap = shardMap;
            }
        }

        private void RegisterNewShard(int tenantId)
        {
            //get or init the shard location using the shard name and server
            Shard shard;
            ShardLocation shardLocation = new ShardLocation(_shardConfiguration.Server, _shardConfiguration.Database, SqlProtocol.Tcp, _shardConfiguration.Port);
            if (!_shardMap.TryGetShard(shardLocation, out shard))
            {
                shard = _shardMap.CreateShard(shardLocation);
            }

            //get or init the point mapping of the organization to the shard
            PointMapping<int> pointMapping;
            if (!_shardMap.TryGetMappingForKey(tenantId, out pointMapping))
            {
                _shardMap.CreatePointMapping(tenantId, shard);
            }
        }

        public SqlConnection OpenConnectionForKey(int tenantId)
        {
            //get the credentials to open a connection to the shard
            var shardCredentials = GetCredentialsConnectionString();

            //get the connection for the shard using the credentials
            var connection = _shardMap.OpenConnectionForKey(tenantId, shardCredentials, ConnectionOptions.Validate);
            return connection;
        }

        internal string GetCredentialsConnectionString()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder()
            {
                UserID = _shardConfiguration.Username,
                Password = _shardConfiguration.Password,
                IntegratedSecurity = false,
                ApplicationName = "TestingEFCoreBehavior",
                ConnectTimeout = 30,
                Encrypt = true,
                MaxPoolSize = 100,
                PersistSecurityInfo = true,
                TrustServerCertificate = true
            };

            return connectionStringBuilder.ConnectionString;
        }
    }
}
