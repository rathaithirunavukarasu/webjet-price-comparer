using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using WebjetPriceComparer.Domain.Entities;
using WebjetPriceComparer.Domain.Enums;
using WebjetPriceComparer.Application.Dtos;
using WebjetPriceComparer.Application.Interfaces;
using WebjetPriceComparer.Application.Mappers;
using WebjetPriceComparer.Application.Services;
using Xunit;

namespace WebjetPriceComparer.Tests
{
    public class MovieCatalogServiceTests
    {
        private readonly Mock<ILogger<MovieCatalogService>> _mockLogger = new();
        private readonly Mock<IMovieProviderRegistry> _mockRegistry = new();
        private readonly Mock<IMovieProviderService> _mockProvider1 = new();
        private readonly Mock<IMovieProviderService> _mockProvider2 = new();
        private readonly MovieCatalogService _service;

        public MovieCatalogServiceTests()
        {
            _mockProvider1.Setup(p => p.Provider).Returns(MovieProvider.Cinemaworld);
            _mockProvider2.Setup(p => p.Provider).Returns(MovieProvider.Filmworld);

            _mockRegistry.Setup(r => r.GetAllProviders()).Returns(new[] { _mockProvider1.Object, _mockProvider2.Object });

            _service = new MovieCatalogService(_mockRegistry.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllMoviesAsync_Returns_Deduplicated_Movies()
        {
            _mockProvider1.Setup(p => p.GetAllMoviesAsync()).ReturnsAsync(new List<MovieOverviewDto>
            {
                new() { Title = "Inception", ID = "1", Year = "2010", Poster = "url", Type = "Movie", Provider = MovieProvider.Cinemaworld },
                new() { Title = "Avatar", ID = "2", Year = "2009", Poster = "url", Type = "Movie", Provider = MovieProvider.Cinemaworld }
            });

            _mockProvider2.Setup(p => p.GetAllMoviesAsync()).ReturnsAsync(new List<MovieOverviewDto>
            {
                new() { Title = "Inception", ID = "3", Year = "2010", Poster = "url", Type = "Movie", Provider = MovieProvider.Filmworld },
                new() { Title = "Titanic", ID = "4", Year = "1997", Poster = "url", Type = "Movie", Provider = MovieProvider.Filmworld }
            });

            var result = await _service.GetAllMoviesAsync();

            Assert.Equal(3, result.Count);
            Assert.Contains(result, m => m.Title == "Inception");
            Assert.Contains(result, m => m.Title == "Avatar");
            Assert.Contains(result, m => m.Title == "Titanic");
        }

        [Fact]
        public async Task ComparePricesAsync_ReturnsComparisonDto_WhenMovieExistsInOneProvider()
        {
            const string title = "Inception";

            _mockProvider1.Setup(p => p.GetAllMoviesAsync()).ReturnsAsync(new List<MovieOverviewDto>
            {
                new() { Title = title, ID = "1", Provider = MovieProvider.Cinemaworld, Poster = "url", Type = "movie", Year = "1900" }
            });

            _mockProvider1.Setup(p => p.GetMovieById("1")).ReturnsAsync(new MovieDetail
            {
                Title = title,
                Year = "2010",
                Price = "10.0",
                Rated = "PG",
                Released = "2010",
                Runtime = "148 min",
                Genre = "Sci-Fi",
                Director = "Christopher Nolan",
                Writer = "Nolan",
                Actors = "Leonardo DiCaprio",
                Plot = "Mind-bending thriller",
                Language = "English",
                Country = "USA",
                Awards = "Oscar",
                Poster = "url",
                Metascore = "74",
                Rating = "8.8",
                Votes = "2M",
                Type = "movie"
            });

            _mockProvider2.Setup(p => p.GetAllMoviesAsync()).ReturnsAsync(new List<MovieOverviewDto>());

            var result = await _service.ComparePricesAsync(title);

            Assert.NotNull(result);
            Assert.Equal(title, result.Title);
            Assert.Single(result.Providers);
            Assert.Equal("10.0", result.Providers[0].Price);
        }

        [Fact]
        public async Task ComparePricesAsync_ReturnsNull_WhenMovieNotFound()
        {
            _mockProvider1.Setup(p => p.GetAllMoviesAsync()).ReturnsAsync(new List<MovieOverviewDto>());
            _mockProvider2.Setup(p => p.GetAllMoviesAsync()).ReturnsAsync(new List<MovieOverviewDto>());

            var result = await _service.ComparePricesAsync("Nonexistent");

            Assert.Null(result);
        }

        [Fact]
        public async Task ComparePricesAsync_SkipsProviders_WithNullDetail()
        {
            const string title = "Inception";

            _mockProvider1.Setup(p => p.GetAllMoviesAsync()).ReturnsAsync(new List<MovieOverviewDto>
            {
                new() { Title = title, ID = "1", Provider = MovieProvider.Cinemaworld, Poster = "url", Type ="movie", Year = "1200" }
            });

            _mockProvider1.Setup(p => p.GetMovieById("1")).ReturnsAsync((MovieDetail?)null);
            _mockProvider2.Setup(p => p.GetAllMoviesAsync()).ReturnsAsync(new List<MovieOverviewDto>());

            var result = await _service.ComparePricesAsync(title);

            Assert.Null(result);
        }
    }
}
