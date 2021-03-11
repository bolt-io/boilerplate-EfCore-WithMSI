using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.EFCore.MSI
{
    public class AzureAdAuthenticationDbConnectionInterceptor : DbConnectionInterceptor
    {

        // note, need to override both Sync and Async methods as EFCore will call both depending on how you interact with EF Core in code. 
        // Don't say I didn't warn you if you are in a hate hole.
        
        private readonly IAzureSqlTokenProvider _azureSqlTokenProvider;

        public AzureAdAuthenticationDbConnectionInterceptor(IAzureSqlTokenProvider azureSqlTokenProvider)
        {
            _azureSqlTokenProvider = azureSqlTokenProvider;
        }

        public override InterceptionResult ConnectionOpening(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
        {
            // this is only suitable for SQL Server. But need to handle postgresql/cosmosDb
            var sqlConnection = (SqlConnection)connection;
            if (DoesConnectionNeedsAccessToken(sqlConnection))
            {
                var (token, _) = _azureSqlTokenProvider.GetAccessToken();
                sqlConnection.AccessToken = token;
            }

            return base.ConnectionOpening(connection, eventData, result);
        }

        public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default)
        {
            var sqlConnection = (SqlConnection)connection;
            if (DoesConnectionNeedsAccessToken(sqlConnection))
            {
                var (token, _) = await _azureSqlTokenProvider.GetAccessTokenAsync(cancellationToken);
                sqlConnection.AccessToken = token;
            }

            return await base.ConnectionOpeningAsync(connection, eventData, result, cancellationToken);
        }

        private static bool DoesConnectionNeedsAccessToken(SqlConnection connection)
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connection.ConnectionString);

            var isAzureDatabase = sqlConnectionStringBuilder.DataSource.Contains("database.windows.net", StringComparison.OrdinalIgnoreCase);
            var isNotUserAuthentication = string.IsNullOrEmpty(sqlConnectionStringBuilder.UserID);

            var isAccessTokenRequired = isAzureDatabase && isNotUserAuthentication;
            return isAccessTokenRequired;
        }
    }
}