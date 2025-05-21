using Microsoft.AspNetCore.Mvc;
using Moq;
using WebjetPriceComparer.Domain.Entities;
using WebjetPriceComparer.Domain.Enums;
using WebjetPriceComparer.Api.Controllers;
using WebjetPriceComparer.Application.Dtos;
using WebjetPriceComparer.Application.Interfaces;
using Xunit;

namespace WebjetPriceComparer.Tests.Controllers
{
    public class MoviesControllerTests
    {
        private readonly Mock<IMovieCatalogService> _mockService;
        private readonly MoviesController _controller;

        public MoviesControllerTests()
        {
            _mockService = new Mock<IMovieCatalogService>();
            _controller = new MoviesController(_mockService.Object);
        }

        [Fact]
        public async Task GetMovies_ReturnsOk_WithMovieList()
        {
            // Arrange
            var movieList = new List<MovieOverviewDto>
            {
                new() { Title = "Inception", ID = "1", Year = "2010", Poster = "url", Type = "Movie", Provider = MovieProvider.Cinemaworld }
            };
            _mockService.Setup(s => s.GetAllMoviesAsync()).ReturnsAsync(movieList);

            // Act
            var result = await _controller.GetMovies();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<List<MovieOverviewDto>>(okResult.Value);
            Assert.Single(data);
            Assert.Equal("Inception", data[0].Title);
        }

        [Fact]
        public async Task GetMovieDetail_ReturnsOk_WithComparisonDto()
        {
            // Arrange
            var title = "Inception";
            var comparisonDto = new MovieComparisonDto
            {
                Title = title,
                Year = "2010",
                Providers = new List<ProviderPriceDto>
                {
                    new() { Provider = "Cinemaworld", Price = "10.00" }
                }
            };
            _mockService.Setup(s => s.ComparePricesAsync(title)).ReturnsAsync(comparisonDto);

            // Act
            var result = await _controller.GetMovieDetail(title);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<MovieComparisonDto>(okResult.Value);
            Assert.Equal(title, data.Title);
            Assert.Single(data.Providers);
        }

        [Fact]
        public async Task GetMovieDetail_ReturnsOk_WithNull_WhenNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.ComparePricesAsync(It.IsAny<string>())).ReturnsAsync((MovieComparisonDto?)null);

            // Act
            var result = await _controller.GetMovieDetail("Unknown");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Null(okResult.Value);
        }
    }
}
