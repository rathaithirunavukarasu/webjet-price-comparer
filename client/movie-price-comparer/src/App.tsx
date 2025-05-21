import { Routes, Route } from 'react-router-dom';
import MovieList from './pages/MovieListPage';
import MovieDetailPage from './pages/MovieDetailPage';

/**
 * Main application component that sets up routing for the movie price comparer app.
 *
 * @returns {JSX.Element} The rendered App component with route definitions.
 */
function App() {
  return (
      <Routes>
        <Route path="/" element={<MovieList />} />
        <Route path="/movies/:title" element={<MovieDetailPage />} />
      </Routes>
  );
}

export default App;
