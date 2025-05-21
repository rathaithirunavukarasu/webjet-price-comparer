namespace WebjetPriceComparer.Infrastructure.Models
{
    /// <summary>
    /// Represents a response containing a list of movie overviews.
    /// </summary>
    public class MovieResponse
    {
        /// <summary>
        /// Gets or sets the list of movie overviews.
        /// </summary>
        public List<MovieOverview> Movies { get; set; }
    }
}