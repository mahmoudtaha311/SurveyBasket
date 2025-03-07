
using Microsoft.Extensions.Caching.Distributed;

namespace SurveyBasket.Api.Services;

public class CacheService(IDistributedCache distributedCache) : ICacheService
{
    private readonly IDistributedCache _distributedCache = distributedCache;

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        var cacheValue = await _distributedCache.GetStringAsync(key, cancellationToken);

        return cacheValue is null ?
             null : JsonSerializer.Deserialize<T>(cacheValue);

    }


    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), cancellationToken);
    }
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }


}
