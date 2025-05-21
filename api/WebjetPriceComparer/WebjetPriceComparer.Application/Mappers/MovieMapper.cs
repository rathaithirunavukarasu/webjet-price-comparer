using WebjetPriceComparer.Domain.Entities;
using WebjetPriceComparer.Domain.Enums;
using WebjetPriceComparer.Application.Dtos;

namespace WebjetPriceComparer.Application.Mappers
{
    /// <summary>
    /// Provides mapping methods for converting movie domain entities to DTOs.
    /// </summary>
    public class MovieMapper
    {
        /// <summary>
        /// Maps a <see cref="MovieDetail"/> and provider responses to a <see cref="MovieComparisonDto"/>.
        /// </summary>
        /// <param name="baseDetail">The base movie detail.</param>
        /// <param name="providerResponses">A list of provider and their corresponding movie details.</param>
        /// <returns>A <see cref="MovieComparisonDto"/> containing movie details and provider price comparisons.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="baseDetail"/> is null.</exception>
        public static MovieComparisonDto MapToComparisonDto(
            MovieDetail baseDetail,
            List<(MovieProvider Provider, MovieDetail? Detail)> providerResponses)
        {
            if (baseDetail == null)
                throw new ArgumentNullException(nameof(baseDetail));

            var comparisonDto = new MovieComparisonDto
            {
                Title = baseDetail.Title,
                Year = baseDetail.Year,
                Rated = baseDetail.Rated,
                Released = baseDetail.Released,
                Runtime = baseDetail.Runtime,
                Genre = baseDetail.Genre,
                Director = baseDetail.Director,
                Writer = baseDetail.Writer,
                Actors = baseDetail.Actors,
                Plot = baseDetail.Plot,
                Language = baseDetail.Language,
                Country = baseDetail.Country,
                Awards = baseDetail.Awards,
                Poster = baseDetail.Poster,
                Metascore = baseDetail.Metascore,
                Rating = baseDetail.Rating,
                Votes = baseDetail.Votes,
                Type = baseDetail.Type,
                Providers = new List<ProviderPriceDto>()
            };

            var sortedProviders = providerResponses
                .Where(p => p.Detail != null && decimal.TryParse(p.Detail.Price, out _))
                .OrderBy(p => decimal.Parse(p.Detail!.Price))
                .ToList();

            foreach (var (provider, detail) in sortedProviders)
            {
                comparisonDto.Providers.Add(new ProviderPriceDto
                {
                    Provider = provider.ToString(),
                    Price = detail!.Price
                });
            }

            return comparisonDto;
        }
    }
}
