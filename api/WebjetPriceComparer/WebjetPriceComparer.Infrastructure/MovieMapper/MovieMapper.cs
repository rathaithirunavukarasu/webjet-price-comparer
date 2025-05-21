using System;
using System.Collections.Generic;
using WebjetPriceComparer.Domain.Entities;
using WebjetPriceComparer.Domain.Enums;
using WebjetPriceComparer.Infrastructure.Models;

namespace WebjetPriceComparer.Infrastructure.MovieMapper
{
    /// <summary>
    /// Provides mapping methods for converting infrastructure movie models to DTOs.
    /// </summary>
    public static class MovieMapper
    {
        /// <summary>
        /// Maps a <see cref="MovieOverview"/> and provider to a <see cref="MovieOverviewDto"/>.
        /// </summary>
        /// <param name="movie">The movie overview model.</param>
        /// <param name="provider">The movie provider.</param>
        /// <returns>A <see cref="MovieOverviewDto"/> representing the movie overview and provider.</returns>
        public static MovieOverviewDto ToOverviewDto(MovieOverview movie, MovieProvider provider)
        {
            return new MovieOverviewDto
            {
                Title = movie.Title,
                Year = movie.Year,
                Poster = movie.Poster,
                Type = movie.Type,
                ID = movie.ID,
                Provider = provider
            };
        }
    }

}
