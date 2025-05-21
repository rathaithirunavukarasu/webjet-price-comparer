using WebjetPriceComparer.Application.Dtos;
using WebjetPriceComparer.Application.Interfaces;
using WebjetPriceComparer.Domain.Entities;
using WebjetPriceComparer.Domain.Enums;
using Microsoft.Extensions.Logging;
using WebjetPriceComparer.Application.Mappers;

namespace WebjetPriceComparer.Application.Services;
public class MovieCatalogService(IMovieProviderRegistry providerRegistry, ILogger<MovieCatalogService> logger) : IMovieCatalogService
{
    private readonly IMovieProviderRegistry _providerRegistry = providerRegistry;
    private readonly ILogger<MovieCatalogService> _logger = logger;

    /// <inheritdoc/>
    public async Task<MovieComparisonDto?> ComparePricesAsync(string movieTitle)
    {
        _logger.LogInformation("Comparing prices for the movie: {MovieTitle}", movieTitle);
        var providerResponses = new List<(MovieProvider Provider, MovieDetail? Detail)>();

        foreach (var provider in _providerRegistry.GetAllProviders())
        {
            var movies = await provider.GetAllMoviesAsync();
            var match = movies?.FirstOrDefault(m => m.Title.Equals(movieTitle, StringComparison.OrdinalIgnoreCase));
            if (match == null)
            {
                _logger.LogInformation("Match not Found for {MovieTitle} in {Provider}", movieTitle, provider.Provider);
                continue;
            }

            var detail = await provider.GetMovieById(match.ID);
            providerResponses.Add((provider.Provider, detail));
        }

        var firstAvailable = providerResponses.FirstOrDefault(p => p.Detail != null).Detail;

        _logger.LogInformation("Found detail for movie {Title} with price {Price}", firstAvailable?.Title, firstAvailable?.Price);

        if (firstAvailable == null)
            return null;

        return MovieMapper.MapToComparisonDto(firstAvailable, providerResponses);
    }

    /// <inheritdoc/>
    public async Task<List<MovieOverviewDto>> GetAllMoviesAsync()
    {
        var allMovies = new List<MovieOverviewDto>();

        foreach (var provider in _providerRegistry.GetAllProviders())
        {
            var movies = await provider.GetAllMoviesAsync();
            if (movies != null)
            {
                allMovies.AddRange(movies);
            }
        }

        return allMovies
            .GroupBy(m => m.Title.ToLowerInvariant())
            .Select(g => g.First())
            .ToList();
    }
}
