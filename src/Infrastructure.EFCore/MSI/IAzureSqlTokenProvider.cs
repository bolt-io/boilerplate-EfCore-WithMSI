
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.EFCore.MSI
{
    public interface IAzureSqlTokenProvider
    {
        (string Token, DateTimeOffset ExpiresOn) GetAccessToken();
        Task<(string Token, DateTimeOffset ExpiresOn)> GetAccessTokenAsync(CancellationToken cancellationToken = default);
    }
}
