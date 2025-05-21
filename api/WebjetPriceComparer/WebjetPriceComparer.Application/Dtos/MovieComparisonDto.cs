namespace WebjetPriceComparer.Application.Dtos
{
    /// <summary>
    /// Data Transfer Object representing detailed information and price comparison for a movie.
    /// </summary>
    public class MovieComparisonDto
    {
        /// <summary>
        /// Gets or sets the movie title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the year the movie was released.
        /// </summary>
        public string Year { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the movie rating (e.g., PG-13).
        /// </summary>
        public string Rated { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the release date of the movie.
        /// </summary>
        public string Released { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the runtime of the movie.
        /// </summary>
        public string Runtime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the genre(s) of the movie.
        /// </summary>
        public string Genre { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the director of the movie.
        /// </summary>
        public string Director { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the writer(s) of the movie.
        /// </summary>
        public string Writer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the actors in the movie.
        /// </summary>
        public string Actors { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the plot summary of the movie.
        /// </summary>
        public string Plot { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the language(s) of the movie.
        /// </summary>
        public string Language { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the country of origin of the movie.
        /// </summary>
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the awards won by the movie.
        /// </summary>
        public string Awards { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the poster image URL of the movie.
        /// </summary>
        public string Poster { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Metascore rating of the movie.
        /// </summary>
        public string Metascore { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rating of the movie.
        /// </summary>
        public string Rating { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of votes the movie has received.
        /// </summary>
        public string Votes { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the movie (e.g., movie, series).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of providers and their prices for the movie.
        /// </summary>
        public List<ProviderPriceDto> Providers { get; set; } = new();
    }
}
