using WebjetPriceComparer.Domain.Entities;
using WebjetPriceComparer.Domain.Enums;

namespace WebjetPriceComparer.Application.Interfaces
{
    /// <summary>
    /// Provides methods for interacting with a specific movie provider service.
    /// </summary>
    public interface IMovieProviderService
    {
        /// <summary>
        /// Gets the movie provider associated with this service.
        /// </summary>
        MovieProvider Provider { get; }

        /// <summary>
        /// Retrieves all movies available from the provider.
        /// </summary>
        /// <returns>A list of <see cref="MovieOverviewDto"/> objects, or null if none found.</returns>
        Task<List<MovieOverviewDto>?> GetAllMoviesAsync();

        /// <summary>
        /// Retrieves detailed information for a movie by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the movie.</param>
        /// <returns>A <see cref="MovieDetail"/> object, or null if not found.</returns>
        Task<MovieDetail?> GetMovieById(string id);
    }
}
