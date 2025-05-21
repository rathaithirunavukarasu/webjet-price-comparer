import React from 'react';
import { render, screen } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import App from '../App';

// 🧪 Mock the pages to isolate routing logic
jest.mock('../pages/MovieListPage', () => () => <div>Mock MovieListPage</div>);
jest.mock('../pages/MovieDetailPage', () => () => <div>Mock MovieDetailPage</div>);

describe('App Routing', () => {
  it('renders MovieListPage at root route "/"', () => {
    render(
      <MemoryRouter initialEntries={['/']}>
        <App />
      </MemoryRouter>
    );

    expect(screen.getByText('Mock MovieListPage')).toBeInTheDocument();
  });

  it('renders MovieDetailPage at route "/movies/Inception"', () => {
    render(
      <MemoryRouter initialEntries={['/movies/Inception']}>
        <App />
      </MemoryRouter>
    );

    expect(screen.getByText('Mock MovieDetailPage')).toBeInTheDocument();
  });

  it('renders nothing or fallback for unknown route', () => {
    render(
      <MemoryRouter initialEntries={['/unknown']}>
        <App />
      </MemoryRouter>
    );

    // You can check for absence of both known pages
    expect(screen.queryByText('Mock MovieListPage')).not.toBeInTheDocument();
    expect(screen.queryByText('Mock MovieDetailPage')).not.toBeInTheDocument();
  });
});
