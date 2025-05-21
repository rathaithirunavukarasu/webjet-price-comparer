using WebjetPriceComparer.Domain.Enums;
using WebjetPriceComparer.Application.Interfaces;

namespace WebjetPriceComparer.Infrastructure.Registry
{
    /// <summary>
    /// Provides a registry for movie provider services.
    /// </summary>
    public class MovieProviderRegistry : IMovieProviderRegistry
    {
        private readonly Dictionary<MovieProvider, IMovieProviderService> _providerMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieProviderRegistry"/> class.
        /// </summary>
        /// <param name="providers">The collection of movie provider services.</param>
        public MovieProviderRegistry(IEnumerable<IMovieProviderService> providers)
        {
            _providerMap = providers.ToDictionary(p => p.Provider);
        }

        /// <inheritdoc/>
        public IMovieProviderService? GetProvider(MovieProvider provider)
        {
            return _providerMap.TryGetValue(provider, out var service) ? service : null;
        }

        /// <inheritdoc/>
        public IEnumerable<IMovieProviderService> GetAllProviders()
        {
            return _providerMap.Values;
        }
    }
}
