import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import MovieCard from '../components/MovieCard';

describe('MovieCard', () => {
  const defaultProps = {
    title: 'Inception',
    year: '2010',
    type: 'movie',
    rated: 'PG-13', 
    released: '2010-07-16',
    runtime: '148 min',
    posterUrl: 'https://example.com/inception.jpg',
  };

  it('renders title and year', () => {
    render(<MovieCard {...defaultProps} onClick={() => {}} />);
    expect(screen.getByText('Inception')).toBeInTheDocument();
    expect(screen.getByText('2010')).toBeInTheDocument();
  });

  it('renders the poster image with correct src and alt', () => {
    render(<MovieCard {...defaultProps} onClick={() => {}} />);
    const image = screen.getByRole('img') as HTMLImageElement;
    expect(image).toBeInTheDocument();
    expect(image.src).toBe(defaultProps.posterUrl);
    expect(image.alt).toBe('Inception');
  });

  it('calls onClick when card is clicked', () => {
    const handleClick = jest.fn();
    render(<MovieCard {...defaultProps} onClick={() => {}} />);
    fireEvent.click(screen.getByRole('img'));
    expect(handleClick).toHaveBeenCalledTimes(1);
  });

  it('replaces image src with placeholder on error', () => {
    const placeholder = 'https://example.com/placeholder.jpg';
    process.env.REACT_APP_PLACEHOLDER_IMAGE = placeholder;

    render(<MovieCard {...defaultProps} onClick={() => {}} />);
    const image = screen.getByRole('img') as HTMLImageElement;

    fireEvent.error(image); // Simulate image load error

    expect(image.src).toBe(placeholder);
  });
});
