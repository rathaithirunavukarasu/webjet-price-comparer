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
/// Provides methods for interacting with the Cinemaworld movie provider API.
/// </summary>
public class CinemaworldApiService : IMovieProviderService
{
    private readonly ApiClient _apiClient;
    private readonly ICacheService _cacheService;
    private const string Base = "cinemaworld";
    private readonly ILogger<CinemaworldApiService> _logger;

    /// <inheritdoc/>
    public MovieProvider Provider => MovieProvider.Cinemaworld;

    /// <summary>
    /// Initializes a new instance of the <see cref="CinemaworldApiService"/> class.
    /// </summary>
    /// <param name="apiClient">The API client for HTTP requests.</param>
    /// <param name="cacheService">The cache service for caching data.</param>
    /// <param name="logger">The logger instance.</param>
    public CinemaworldApiService(ApiClient apiClient, ICacheService cacheService, ILogger<CinemaworldApiService> logger)
    {
        _apiClient = apiClient;
        _cacheService = cacheService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<List<MovieOverviewDto>?> GetAllMoviesAsync()
    {
        _logger.LogInformation("Fetching all movies for Cinemaworld");
        return await _cacheService.GetOrSetAsync("Cinemaworld_AllMovies", () =>
            RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                var response = await _apiClient.GetAsync($"api/{Base}/movies");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to fetch movies from Cinemaworld API. Status code: {StatusCode}", response.StatusCode);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<MovieResponse>(json);
                return data?.Movies.Select(m => MovieMapper.ToOverviewDto(m, Provider)).ToList();
            }));
    }

    /// <inheritdoc/>
    public async Task<MovieDetail?> GetMovieById(string id)
    {
        string cacheKey = $"Cinemaworld_MovieDetail_{id}";
        _logger.LogInformation("Fetching movie details for ID: {MovieId} from Cinemaworld", id);
        return await _cacheService.GetOrSetAsync(cacheKey, () =>
            RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                var response = await _apiClient.GetAsync($"api/{Base}/movie/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to fetch movie details for ID: {MovieId} from Cinemaworld API", id);
                    return null;
                }
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<MovieDetail>(json);
                return data;
            }));
    }
}
