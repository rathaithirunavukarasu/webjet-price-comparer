import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import MovieListPage from '../pages/MovieListPage';
import fetchMock from 'jest-fetch-mock';

// Mock react-router-dom before importing the component
jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useNavigate: () => jest.fn(),
}));

beforeEach(() => {
  fetchMock.resetMocks();
});

describe('MovieListPage', () => {
  it('shows loading indicator initially', () => {
    fetchMock.mockResponseOnce(() => new Promise(() => {})); // never resolves
    render(<MovieListPage />);
    expect(screen.getByText(/Loading.../i)).toBeInTheDocument();
  });

  it('displays error message if fetch fails', async () => {
    fetchMock.mockRejectOnce(new Error('Fetch failed'));
    render(<MovieListPage />);
    expect(await screen.findByText(/Fetch failed/i)).toBeInTheDocument();
  });

  it('renders movies when fetch succeeds', async () => {
    const mockMovies = [
      { id: '1', title: 'Movie One', year: '2023', poster: 'poster1.jpg' },
      { id: '2', title: 'Movie Two', year: '2024', poster: 'poster2.jpg' },
    ];

    fetchMock.mockResponseOnce(JSON.stringify(mockMovies));
    render(<MovieListPage />);

    await waitFor(() => {
      expect(screen.getByText('Available Movies')).toBeInTheDocument();
    });

    expect(screen.getByText('Movie One')).toBeInTheDocument();
    expect(screen.getByText('Movie Two')).toBeInTheDocument();

    const images = screen.getAllByRole('img');
    expect(images).toHaveLength(2);
    expect(images[0]).toHaveAttribute('src', 'poster1.jpg');
    expect(images[1]).toHaveAttribute('src', 'poster2.jpg');
  });
});