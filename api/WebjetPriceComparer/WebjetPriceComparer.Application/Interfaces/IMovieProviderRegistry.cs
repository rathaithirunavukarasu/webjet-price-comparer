using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebjetPriceComparer.Domain.Enums;

namespace WebjetPriceComparer.Application.Interfaces
{
    /// <summary>
    /// Provides methods to retrieve movie provider services.
    /// </summary>
    public interface IMovieProviderRegistry
    {
        /// <summary>
        /// Gets the movie provider service for the specified provider.
        /// </summary>
        /// <param name="provider">The movie provider.</param>
        /// <returns>The <see cref="IMovieProviderService"/> instance, or null if not found.</returns>
        public IMovieProviderService? GetProvider(MovieProvider provider);

        /// <summary>
        /// Gets all registered movie provider services.
        /// </summary>
        /// <returns>An enumerable of <see cref="IMovieProviderService"/>.</returns>
        public IEnumerable<IMovieProviderService> GetAllProviders();
    }
}
