using WebjetPriceComparer.Domain.Entities;
using WebjetPriceComparer.Application.Dtos;

namespace WebjetPriceComparer.Application.Interfaces
{
    /// <summary>
    /// Provides methods for retrieving movie information and comparing prices.
    /// </summary>
    public interface IMovieCatalogService
    {
        /// <summary>
        /// Retrieves a list of all available movies.
        /// </summary>
        /// <returns>A list of <see cref="MovieOverviewDto"/> objects.</returns>
        Task<List<MovieOverviewDto>> GetAllMoviesAsync();

        /// <summary>
        /// Compares prices for a specific movie by its identifier.
        /// </summary>
        /// <param name="movieId">The identifier of the movie to compare prices for.</param>
        /// <returns>A <see cref="MovieComparisonDto"/> with price comparison details, or null if not found.</returns>
        Task<MovieComparisonDto?> ComparePricesAsync(string movieId);
    }
}
