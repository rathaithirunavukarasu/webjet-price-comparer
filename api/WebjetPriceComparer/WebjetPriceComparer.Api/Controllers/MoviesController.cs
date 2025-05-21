using Microsoft.AspNetCore.Mvc;
using WebjetPriceComparer.Application.Interfaces;

namespace WebjetPriceComparer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController(IMovieCatalogService movieComparisonService) : ControllerBase
    {
        private readonly IMovieCatalogService _movieComparisonService = movieComparisonService;

        /// <summary>
        /// Retrieves a list of all movies.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of movies.</returns>
        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var result = await _movieComparisonService.GetAllMoviesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Retrieves movie price comparison details for a given title.
        /// </summary>
        /// <param name="title">The title of the movie to compare prices for.</param>
        /// <returns>An <see cref="IActionResult"/> containing the price comparison details.</returns>
        [HttpGet("comparison")]
        public async Task<IActionResult> GetMovieDetail([FromQuery] string title)
        {
            var result = await _movieComparisonService.ComparePricesAsync(title);
            return Ok(result);
        }
    }
}
