using Microsoft.Extensions.Logging;
using System.Text.Json;
using WebjetPriceComparer.Domain.Entities;
using WebjetPriceComparer.Domain.Enums;
using WebjetPriceComparer.Application.Interfaces;
using WebjetPriceComparer.Infrastructure.Helper;
using WebjetPriceComparer.Infrastructure.Models;
using WebjetPriceComparer.Infrastructure.MovieMapper;

namespace WebjetPriceComparer.Application.Services;

/// <summary>
/// Provides methods for interacting with the Filmworld movie provider API.
/// </summary>
public class FilmworldApiService : IMovieProviderService
{
    private readonly ApiClient _apiClient;
    private readonly ICacheService _cacheService;
    private const string Base = "filmworld";
    private readonly ILogger<FilmworldApiService> _logger;

    /// <inheritdoc/>
    public MovieProvider Provider => MovieProvider.Filmworld;

    /// <summary>
    /// Initializes a new instance of the <see cref="FilmworldApiService"/> class.
    /// </summary>
    /// <param name="apiClient">The API client for HTTP requests.</param>
    /// <param name="cacheService">The cache service for caching data.</param>
    /// <param name="logger">The logger instance.</param>
    public FilmworldApiService(ApiClient apiClient, ICacheService cacheService, ILogger<FilmworldApiService> logger)
    {
        _apiClient = apiClient;
        _cacheService = cacheService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<List<MovieOverviewDto>?> GetAllMoviesAsync()
    {
        _logger.LogInformation("Fetching all movies from Filmworld");
        return await _cacheService.GetOrSetAsync("Filmworld_AllMovies", () =>
            RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                var response = await _apiClient.GetAsync($"api/{Base}/movies");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to fetch movies from Filmworld API. Status code: {StatusCode}", response.StatusCode);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<MovieResponse>(json);
                return data?.Movies.Select(m => MovieMapper.ToOverviewDto(m, Provider)).ToList();
            }));
    }

    /// <inheritdoc/>
    public async Task<MovieDetail?> GetMovieById(string title)
    {
        string cacheKey = $"Filmworld_MovieDetail_{title}";
        _logger.LogInformation("Fetching movie details for ID: {MovieTitle} from Filmworld", title);
        return await _cacheService.GetOrSetAsync(cacheKey, () =>
            RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                var response = await _apiClient.GetAsync($"api/{Base}/movie/{title}");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to fetch movie details from Filmworld API. Status code: {StatusCode}", response.StatusCode);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<MovieDetail>(json);
                return data;
            }));
    }
}
