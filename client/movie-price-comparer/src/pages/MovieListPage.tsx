import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { fetchMovieList, Movie } from "../services/movie";
import MovieCard from "../components/MovieCard";

/**
 * Displays a list of available movies.
 *
 * Fetches the movie list, handles loading and error states,
 * and renders MovieCard components for each movie.
 *
 * @returns {JSX.Element} The rendered MovieListPage component.
 */
const MovieListPage: React.FC = () => {
  const navigate = useNavigate();
  const [movies, setMovies] = useState<Movie[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadMovies = async () => {
      setLoading(true);
      try {
        const movieList = await fetchMovieList();
        setMovies(movieList);
      } catch (err: any) {
        setError(err.message || "Unexpected error");
      } finally {
        setLoading(false);
      }
    };

    loadMovies();
  }, []);

  if (loading) return <div className="text-center mt-10 text-lg">Loading...</div>;
  if (error) return <div className="text-center text-red-600 mt-10">{error}</div>;

  return (
    <div className="px-6 py-8">
      <h1 className="text-2xl font-bold mb-6 text-center">Available Movies</h1>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 xl:grid-cols-4 gap-6">
        {movies.map((movie) => (
          <MovieCard
            key={movie.ID}
            title={movie.Title}
            year={movie.Year}
            onClick={() => navigate(`/movies/${encodeURIComponent(movie.Title)}`)}
            posterUrl={movie.Poster}
          />
        ))}
      </div>
    </div>
  );
};

export default MovieListPage;
