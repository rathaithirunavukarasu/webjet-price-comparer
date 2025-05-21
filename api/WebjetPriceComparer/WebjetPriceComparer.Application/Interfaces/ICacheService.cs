namespace WebjetPriceComparer.Application.Interfaces
{
    /// <summary>
    /// Provides caching functionality for storing and retrieving data.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Retrieves the cached value for the specified key, or sets it using the provided delegate if not present.
        /// </summary>
        /// <typeparam name="T">The type of the value to cache.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="getData">A delegate to retrieve the data if not cached.</param>
        /// <param name="cacheSeconds">The cache duration in seconds (default is 300).</param>
        /// <returns>The cached or newly retrieved value, or null if not found.</returns>
        Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> getData, int cacheSeconds = 300);
    }
}
