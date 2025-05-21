using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebjetPriceComparer.Infrastructure.Helper
{
    /// <summary>
    /// Provides HTTP client helper methods for making API requests.
    /// </summary>
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client instance.</param>
        /// <param name="config">The configuration instance.</param>
        public ApiClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Sends a GET request to the specified URL.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <returns>The HTTP response message.</returns>
        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _httpClient.GetAsync(url);
        }

        /// <summary>
        /// Sends a GET request to the specified URL and deserializes the JSON response to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response to.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <returns>The deserialized response object, or <c>default</c> if the request fails.</returns>
        public async Task<T?> GetAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode) return default;

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch
            {
                return default;
            }
        }
    }
}
