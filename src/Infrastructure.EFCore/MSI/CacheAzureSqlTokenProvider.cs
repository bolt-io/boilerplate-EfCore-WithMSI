using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.EFCore.MSI
{
    public class AzureSqlTokenProviderCacheWrapper : IAzureSqlTokenProvider
    {
        private static readonly string _cacheKey = $"{nameof(AzureSqlTokenProviderCacheWrapper)}.{nameof(GetAccessTokenAsync)}";

        private readonly IAzureSqlTokenProvider _inner;
        private readonly IMemoryCache _cache; // todo; will this work in the "cloud"

        public AzureSqlTokenProviderCacheWrapper(IAzureSqlTokenProvider inner, IMemoryCache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public (string Token, DateTimeOffset ExpiresOn) GetAccessToken()
        {
            return _cache.GetOrCreate(_cacheKey, entry =>
            {
                var (token, expiresOn) = _inner.GetAccessToken();

                entry.SetAbsoluteExpiration(expiresOn);

                return (token, expiresOn);
            });
        }

        public async Task<(string Token, DateTimeOffset ExpiresOn)> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            return await _cache.GetOrCreateAsync(_cacheKey, async entry =>
            {
                var (token, expiresOn) = await _inner.GetAccessTokenAsync(cancellationToken);

                entry.SetAbsoluteExpiration(expiresOn);

                return (token, expiresOn);
            });
        }
    }
}