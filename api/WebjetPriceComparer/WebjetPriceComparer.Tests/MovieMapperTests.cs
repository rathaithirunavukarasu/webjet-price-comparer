using System;
using System.Collections.Generic;
using System.Linq;
using WebjetPriceComparer.Domain.Entities;
using WebjetPriceComparer.Domain.Enums;
using WebjetPriceComparer.Application.Dtos;
using WebjetPriceComparer.Application.Mappers;
using Xunit;

namespace WebjetPriceComparer.Tests.Mappers
{
    public class MovieMapperTests
    {
        [Fact]
        public void MapToComparisonDto_ReturnsDto_WithSortedPrices()
        {
            // Arrange
            var baseDetail = new MovieDetail
            {
                Title = "Inception",
                Year = "2010",
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
            };

            var providerResponses = new List<(MovieProvider, MovieDetail?)>
            {
                (MovieProvider.Cinemaworld, new MovieDetail { Price = "12.00" }),
                (MovieProvider.Filmworld, new MovieDetail { Price = "10.00" }),
                (MovieProvider.Cinemaworld, new MovieDetail { Price = "11.00" }) // test duplicate provider case
            };

            // Act
            var result = MovieMapper.MapToComparisonDto(baseDetail, providerResponses);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Inception", result.Title);
            Assert.Equal(3, result.Providers.Count);
            Assert.Equal("10.00", result.Providers[0].Price);
            Assert.Equal("11.00", result.Providers[1].Price);
            Assert.Equal("12.00", result.Providers[2].Price);
        }

        [Fact]
        public void MapToComparisonDto_Throws_WhenBaseDetailIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                MovieMapper.MapToComparisonDto(null!, new List<(MovieProvider, MovieDetail?)>())
            );
        }

        [Fact]
        public void MapToComparisonDto_SkipsProviders_WithInvalidPrice()
        {
            // Arrange
            var baseDetail = new MovieDetail { Title = "Inception" };

            var providerResponses = new List<(MovieProvider, MovieDetail?)>
            {
                (MovieProvider.Cinemaworld, new MovieDetail { Price = "invalid" }),
                (MovieProvider.Filmworld, new MovieDetail { Price = null }),
                (MovieProvider.Filmworld, null),
                (MovieProvider.Cinemaworld, new MovieDetail { Price = "10.00" })
            };

            // Act
            var result = MovieMapper.MapToComparisonDto(baseDetail, providerResponses);

            // Assert
            Assert.Single(result.Providers);
            Assert.Equal("10.00", result.Providers[0].Price);
        }
    }
}
