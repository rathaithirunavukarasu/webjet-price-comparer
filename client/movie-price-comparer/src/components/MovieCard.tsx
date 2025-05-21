import React from "react";

/**
 * Props for the MovieCard component.
 */
interface MovieCardProps {
  /** The title of the movie. */
  title: string;
  /** The release year of the movie. */
  year: string;
  /** The URL of the movie poster image. */
  posterUrl: string;
  /** Optional click handler for the card. */
  onClick: () => void;
}

/**
 * Displays a card with movie poster, title, and year.
 *
 * @param {MovieCardProps} props - The props for the component.
 * @returns {JSX.Element} The rendered MovieCard component.
 */
const MovieCard: React.FC<MovieCardProps> = ({
  title,
  year,
  posterUrl,
  onClick,
}) => {
  return (
    <div
      className="max-w-xs bg-white rounded-2xl shadow-md overflow-hidden hover:shadow-xl transition-shadow cursor-pointer"
      onClick={onClick}
    >
      <img
        src={posterUrl}
        alt={title}
        className="w-full rounded-lg shadow-md"
        onError={(e) => {
          (e.target as HTMLImageElement).src = `${process.env.REACT_APP_PLACEHOLDER_IMAGE}`;
        }}
      />
      <div className="p-4">
        <h2 className="text-lg font-bold text-gray-800">{title}</h2>
        <p className="text-sm text-gray-600">{year}</p>
      </div>
    </div>
  );
};

export default MovieCard;
