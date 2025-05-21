import React from 'react';
import { render, screen } from '@testing-library/react';
import MovieDetailPage from '../pages/MovieDetailPage';
import * as movieService from '../services/movie';
import { MemoryRouter, Route, Routes } from 'react-router-dom';

jest.mock('../services/movie');
const mockFetchMovieDetail = movieService.fetchMovieDetail as jest.Mock;

jest.mock('react-router-dom', () => {
  const actual = jest.requireActual('react-router-dom');
  return {
    ...actual,
    useParams: () => ({ title: 'MovieTitle' }),
  };
});

describe('MovieDetailPage', () => {
  const setup = () => {
    return render(
      <MemoryRouter initialEntries={['/movies/MovieTitle']}>
        <Routes>
          <Route path="/movies/:title" element={<MovieDetailPage />} />
        </Routes>
      </MemoryRouter>
    );
  };

  const mockMovie = {
    title: 'MovieTitle',
    year: '2010',
    poster: 'https://example.com/MovieTitle.jpg',
    rated: 'PG-13',
    released: '2010-07-16',
    runtime: '148 min',
    genre: 'Action, Sci-Fi',
    director: 'Christ',
    writer: 'Chris',
    actors: 'Leo',
    plot: 'plot',
    language: 'English',
    country: 'USA',
    awards: 'Oscar',
    metascore: '74',
    rating: '8.8',
    votes: '2000000',
    type: 'movie',
    providers: [
      { provider: 'Cinemaworld', price: 10.0 },
      { provider: 'Filmworld', price: 8.5 },
    ],
  };

  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('shows loading indicator initially', async () => {
    mockFetchMovieDetail.mockImplementation(() => new Promise(() => {}));
    setup();
    expect(screen.getByText(/loading/i)).toBeInTheDocument();
  });

  it('shows movie not found on fetch failure', async () => {
    mockFetchMovieDetail.mockRejectedValueOnce(new Error('Not found'));
    setup();
    expect(await screen.findByText(/movie not found/i)).toBeInTheDocument();
  });

  it('renders movie details with best price highlighted', async () => {
    mockFetchMovieDetail.mockResolvedValueOnce(mockMovie);
    setup();

    expect(await screen.findByText('MovieTitle')).toBeInTheDocument();
    expect(screen.getByText(/Action, Sci-Fi/)).toBeInTheDocument();
    expect(screen.getByText('$10.00')).toBeInTheDocument();
    expect(screen.getByText('$8.50')).toBeInTheDocument();
    expect(screen.getByText(/Best Price/)).toBeInTheDocument();
  });
});
