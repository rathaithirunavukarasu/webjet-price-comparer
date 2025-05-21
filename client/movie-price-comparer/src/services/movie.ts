export interface Movie {
  ID: string;
  Title: string;
  Year: string;
  Poster: string;
  Type: string;
}

/**
 * Fetches the list of movies from the API.
 * @returns {Promise<Movie[]>} A promise that resolves to an array of movies.
 */
export const fetchMovieList = async (): Promise<Movie[]> => {
  const response = await fetch(`${process.env.REACT_APP_MOVIE_API}/movies`);
  if (!response.ok) {
    throw new Error("Failed to fetch movies");
  }

  const data = await response.json();
  return data.map((m: any) => ({
    ID: m.id,
    Title: m.title,
    Year: m.year,
    Poster: m.poster,
    Type: m.type,
  }));
};

export type ProviderPrice = {
  provider: string;
  price: number;
};

export interface MovieDetail {
  title: string;
  year: string;
  rated: string;
  released: string;
  runtime: string;
  genre: string;
  director: string;
  writer: string;
  actors: string;
  plot: string;
  language: string;
  country: string;
  awards: string;
  poster: string;
  metascore: string;
  rating: string;
  votes: string;
  type: string;
  providers: ProviderPrice[];
}

/**
 * Fetches detailed information and price comparison for a movie by title.
 * @param {string} urlEncodedTitle - The URL-encoded title of the movie.
 * @returns {Promise<MovieDetail>} A promise that resolves to the movie detail.
 */
export const fetchMovieDetail = async (urlEncodedTitle: string): Promise<MovieDetail> => {
  const response = await fetch(`${process.env.REACT_APP_MOVIE_API}/movies/comparison?title=${urlEncodedTitle}`);
  if (!response.ok) {
    throw new Error("Failed to fetch movie detail");
  }

  const data = await response.json();
  return {
    ...data,
    providers: data.providers.map((p: any) => ({
      provider: p.provider,
      price: parseFloat(p.price),
    })),
  };
};

