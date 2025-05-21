using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using WebjetPriceComparer.Application.Interfaces;

namespace WebjetPriceComparer.Infrastructure.Caching;

/// <summary>
/// Provides Redis-based caching functionality for storing and retrieving data.
/// </summary>
public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisCacheService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RedisCacheService"/> class.
    /// </summary>
    /// <param name="cache">The distributed cache instance.</param>
    /// <param name="logger">The logger instance.</param>
    public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> getData, int cacheSeconds = 300)
    {
        var cached = await _cache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
        {
            _logger.LogInformation("Cache hit for key: {CacheKey}", key);
            return JsonSerializer.Deserialize<T>(cached);
        }

        var data = await getData();

        if (data is not null && !IsEmpty(data))
        {
            var json = JsonSerializer.Serialize(data);
            await _cache.SetStringAsync(key, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheSeconds)
            });

            _logger.LogInformation("Cached data for key: {CacheKey}", key);
        }
        else
        {
            _logger.LogInformation("Skipping cache set for key: {CacheKey} because data is null or empty", key);
        }

        return data;
    }

    /// <summary>
    /// Determines whether the provided data is empty.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <param name="data">The data to check.</param>
    /// <returns><c>true</c> if the data is empty; otherwise, <c>false</c>.</returns>
    private static bool IsEmpty<T>(T data)
    {
        if (data is string s)
            return string.IsNullOrWhiteSpace(s);

        if (data is IEnumerable<object> collection)
            return !collection.Any();

        return false;
    }
}
