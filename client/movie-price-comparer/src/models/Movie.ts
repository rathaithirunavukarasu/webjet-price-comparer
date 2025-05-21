/**
 * Represents a summary of a movie for listing purposes.
 */
export interface MovieSummary {
  /** The title of the movie. */
  title: string;
  /** The release year of the movie. */
  year: number;
  /** The unique identifier for the movie. */
  movieId: string;
  /** The cheapest price available for the movie. */
  cheapestPrice: number;
  /** The provider offering the cheapest price. */
  cheapestProvider: string;
}

/**
 * Represents detailed information about a movie, including provider prices.
 */
export interface MovieDetail {
  /** The title of the movie. */
  title: string;
  /** The release year of the movie. */
  year: number;
  /** The list of providers and their prices for the movie. */
  providers: {
    /** The provider name. */
    provider: string;
    /** The price offered by the provider. */
    price: number;
    /** The currency of the price. */
    currency: string;
  }[];
  /** The cheapest provider and price for the movie. */
  cheapest: {
    /** The provider offering the cheapest price. */
    provider: string;
    /** The cheapest price. */
    price: number;
  };
}
