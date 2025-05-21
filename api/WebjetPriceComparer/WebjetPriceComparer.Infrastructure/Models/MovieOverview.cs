namespace WebjetPriceComparer.Infrastructure.Models
{
    /// <summary>
    /// Represents a basic overview of a movie.
    /// </summary>
    public class MovieOverview
    {
        /// <summary>
        /// Gets or sets the movie title.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the year the movie was released.
        /// </summary>
        public required string Year { get; set; }

        /// <summary>
        /// Gets or sets the type of the movie (e.g., movie, series).
        /// </summary>
        public required string Type { get; set; }

        /// <summary>
        /// Gets or sets the poster image URL of the movie.
        /// </summary>
        public required string Poster { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the movie.
        /// </summary>
        public required string ID { get; set; }
    }
}
