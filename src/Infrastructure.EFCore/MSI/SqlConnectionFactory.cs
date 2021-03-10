

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Azure.Identity;
using Azure.Core;

//var isAzureDatabase = sqlConnectionStringBuilder.DataSource.Contains("database.windows.net", StringComparison.OrdinalIgnoreCase);
//var isNotUserAuthentication = string.IsNullOrEmpty(sqlConnectionStringBuilder.UserID);

namespace Infrastructure.EFCore.MSI
{
    public class SqlConnectionFactory
    {
        private readonly string _connectionString;
        private readonly IAzureSqlTokenProvider _azureSqlTokenProvider;

        public SqlConnectionFactory(string connectionString, IAzureSqlTokenProvider azureSqlTokenProvider)
        {
            _connectionString = connectionString;
            _azureSqlTokenProvider = azureSqlTokenProvider;
        }

        public async Task<SqlConnection> CreateConnection()
        {
            var sqlConnection = new SqlConnection(_connectionString);

            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(_connectionString);

            var isAzureDatabase = sqlConnectionStringBuilder.DataSource.Contains("database.windows.net", StringComparison.OrdinalIgnoreCase);
            var isNotUserAuthentication = string.IsNullOrEmpty(sqlConnectionStringBuilder.UserID);
            
            if(isAzureDatabase && isNotUserAuthentication)
            {
                var credential = new ChainedTokenCredential(new ManagedIdentityCredential(), new AzureCliCredential());
                
                var tokenRequest = new TokenRequestContext(new[] { "https://database.windows.net//.default" }); // double slash here is not a typo
                var tokenResponse = await credential.GetTokenAsync(tokenRequest);

                sqlConnection.AccessToken = tokenResponse.Token;
            }
            
            return sqlConnection;
        }
    }
}