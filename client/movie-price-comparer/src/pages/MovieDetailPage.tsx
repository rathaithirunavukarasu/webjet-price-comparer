import React, { useEffect, useState } from "react";
import { fetchMovieDetail, MovieDetail } from "../services/movie";
import { useParams } from "react-router-dom";

/**
 * Displays detailed information and price comparison for a selected movie.
 *
 * Fetches movie details based on the title from the URL params,
 * shows loading and error states, and highlights the best price among providers.
 *
 * @returns {JSX.Element} The rendered MovieDetailPage component.
 */
const MovieDetailPage: React.FC = () => {
  const { title } = useParams<{ title: string }>();
  const [movie, setMovie] = useState<MovieDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [bestPrice, setBestPrice] = useState<number | null>(null);

  useEffect(() => {
    const loadMovie = async () => {
      try {
        const data = await fetchMovieDetail(title || "");
        setMovie(data);

        if (data.providers && data.providers.length > 0) {
          const best = Math.min(...data.providers.map((p) => p.price));
          setBestPrice(best);
        }
      } catch (error) {
        console.error("Failed to load movie:", error);
      } finally {
        setLoading(false);
      }
    };

    loadMovie();
  }, [title]);

  if (loading) return <div className="p-6 text-center">Loading...</div>;
  if (!movie) return <div className="p-6 text-center">Movie not found.</div>;

  return (
    <div className="p-6 max-w-5xl mx-auto">
      <h1 className="text-3xl font-bold mb-4">{movie.title}</h1>
      <div className="flex flex-col md:flex-row gap-6">
        {/* Poster */}
        <div className="w-full md:w-1/3">
          {movie.poster && (
            <img
              src={movie.poster}
              alt={movie.title}
              className="w-full rounded-lg shadow-md"
              onError={(e) => {
                (e.target as HTMLImageElement).src = `${process.env.REACT_APP_PLACEHOLDER_IMAGE}`;
              }}
            />
          )}

          {/* Price Comparison */}
          {movie.providers?.length > 0 && (
            <div className="mt-4 space-y-2">
              {movie.providers.map((p, i) => (
                <div
                  key={i}
                  className={`p-3 border rounded shadow transition-all ${
                    p.price === bestPrice
                      ? "bg-green-50 border-l-4 border-green-500 shadow-lg"
                      : "bg-white"
                  }`}
                >
                  <div className="flex justify-between items-center">
                    <span className="font-medium">{p.provider}</span>
                    <span className="font-semibold text-lg">
                      ${p.price.toFixed(2)}
                    </span>
                  </div>
                  {p.price === bestPrice && (
                    <div className="text-green-700 font-semibold text-sm mt-1">
                      Best Price
                    </div>
                  )}
                </div>
              ))}
            </div>
          )}
        </div>

        {/* Movie Info */}
        <div className="w-full md:w-2/3 space-y-2">
          {movie.year && <p><strong>Year:</strong> {movie.year}</p>}
          {movie.rated && <p><strong>Rated:</strong> {movie.rated}</p>}
          {movie.released && <p><strong>Released:</strong> {movie.released}</p>}
          {movie.runtime && <p><strong>Runtime:</strong> {movie.runtime}</p>}
          {movie.genre && <p><strong>Genre:</strong> {movie.genre}</p>}
          {movie.director && <p><strong>Director:</strong> {movie.director}</p>}
          {movie.writer && <p><strong>Writer:</strong> {movie.writer}</p>}
          {movie.actors && <p><strong>Actors:</strong> {movie.actors}</p>}
          {movie.plot && <p><strong>Plot:</strong> {movie.plot}</p>}
          {movie.language && <p><strong>Language:</strong> {movie.language}</p>}
          {movie.country && <p><strong>Country:</strong> {movie.country}</p>}
          {movie.awards && <p><strong>Awards:</strong> {movie.awards}</p>}
          {movie.metascore && <p><strong>Metascore:</strong> {movie.metascore}</p>}
          {movie.rating && <p><strong>Rating:</strong> {movie.rating}</p>}
          {movie.votes && <p><strong>Votes:</strong> {movie.votes}</p>}
          {movie.type && <p><strong>Type:</strong> {movie.type}</p>}
        </div>
      </div>
    </div>
  );
};

export default MovieDetailPage;
