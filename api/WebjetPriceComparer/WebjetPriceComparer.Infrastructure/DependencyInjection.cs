using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebjetPriceComparer.Application.Interfaces;
using WebjetPriceComparer.Application.Services;
using WebjetPriceComparer.Infrastructure.Caching;
using WebjetPriceComparer.Infrastructure.Helper;
using WebjetPriceComparer.Infrastructure.Registry;

namespace WebjetPriceComparer.Infrastructure.DI;

/// <summary>
/// Provides extension methods for registering infrastructure services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds infrastructure services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="config">The application configuration.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config.GetConnectionString("Redis");
            options.InstanceName = "WebjetCache_";
        });

        // Caching service  
        services.AddScoped<ICacheService, RedisCacheService>();

        // ApiClient using HttpClientFactory  
        services.AddHttpClient<ApiClient>((provider, client) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var baseUrl = configuration["WebjetApi:BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException("The WebjetApi:BaseUrl configuration is missing or empty.");
            }
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("x-access-token", configuration["WebjetApi:ApiToken"]);
        });

        // Movie providers  
        services.AddScoped<IMovieProviderService, CinemaworldApiService>();
        services.AddScoped<IMovieProviderService, FilmworldApiService>();
        services.AddScoped<CinemaworldApiService>();
        services.AddScoped<FilmworldApiService>();
        services.AddScoped<IMovieProviderRegistry, MovieProviderRegistry>();

        return services;
    }
}
