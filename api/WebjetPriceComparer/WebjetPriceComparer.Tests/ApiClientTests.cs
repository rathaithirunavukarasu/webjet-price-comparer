using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using WebjetPriceComparer.Infrastructure.Helper;

namespace WebjetPriceComparer.Tests.Helpers
{
    public class ApiClientTests
    {
        private ApiClient CreateApiClient(HttpResponseMessage httpResponse)
        {
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://mockapi.test/")
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "WebjetApi:ApiToken", "mock-token" },
                    { "WebjetApi:BaseUrl", "https://mockapi/" }
                })
                .Build();

            return new ApiClient(httpClient, config);
        }

        public class SampleDto
        {
            public string? Name { get; set; }
        }

        [Fact]
        public async Task GetAsync_ReturnsRawHttpResponse()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("test-response")
            };
            var apiClient = CreateApiClient(response);

            // Act
            var result = await apiClient.GetAsync("sample/endpoint");

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task GetAsync_Generic_ReturnsDeserializedObject()
        {
            // Arrange
            var sample = new SampleDto { Name = "TestName" };
            var json = JsonSerializer.Serialize(sample);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var apiClient = CreateApiClient(response);

            // Act
            var result = await apiClient.GetAsync<SampleDto>("sample/endpoint");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TestName", result!.Name);
        }

        [Fact]
        public async Task GetAsync_Generic_ReturnsNull_OnHttpError()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var apiClient = CreateApiClient(response);

            // Act
            var result = await apiClient.GetAsync<SampleDto>("sample/endpoint");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAsync_Generic_ReturnsNull_OnException()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("network error"));

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://mockapi.test/")
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "WebjetApi:ApiToken", "mock-token" },
                    { "WebjetApi:BaseUrl", "https://mockapi.test/" }
                })
                .Build();

            var apiClient = new ApiClient(httpClient, config);

            // Act
            var result = await apiClient.GetAsync<SampleDto>("sample/endpoint");

            // Assert
            Assert.Null(result);
        }
    }
}
