using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;

namespace Infrastructure.EFCore.MSI
{
    public class AzureIdentityAzureSqlTokenProvider : IAzureSqlTokenProvider
    {
        private static readonly TokenCredential _credential = new ChainedTokenCredential(new ManagedIdentityCredential()
                                                                                            , new AzureCliCredential());

        // See https://docs.microsoft.com/azure/active-directory/managed-identities-azure-resources/services-support-managed-identities#azure-sql
        private static readonly string[] _azureSqlScopes = new string[] { "https://database.windows.net//.default" };

        public (string Token, DateTimeOffset ExpiresOn) GetAccessToken()
        {
            var tokenRequest = new TokenRequestContext(_azureSqlScopes);
            var tokenResult = _credential.GetToken(tokenRequest, default);

            return (tokenResult.Token, tokenResult.ExpiresOn);
        }

        public async Task<(string Token, DateTimeOffset ExpiresOn)> GetAccessTokenAsync(CancellationToken cancellationToken = default)
        {
            var tokenRequest = new TokenRequestContext(_azureSqlScopes);
            var tokenResult = await _credential.GetTokenAsync(tokenRequest, cancellationToken);

            return (tokenResult.Token, tokenResult.ExpiresOn);
        }
    }
}