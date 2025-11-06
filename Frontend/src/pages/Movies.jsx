import { useState, useEffect, useMemo } from 'react';
import { movieService } from '../services/movieService';
import { genreService } from '../services/genreService';
import Header from '../components/Header';
import MovieCard from '../components/MovieCard';
import './Browse.css';

const Movies = () => {
  const [allMovies, setAllMovies] = useState([]);
  const [genres, setGenres] = useState([]);
  const [selectedGenre, setSelectedGenre] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const [genresRes, moviesRes] = await Promise.all([
        genreService.getAll(),
        movieService.getAll()
      ]);
      setGenres(genresRes.data || []);
      setAllMovies(moviesRes.data || []);
    } catch (err) {
      setError('Failed to load movies');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  // Filter movies on the client side based on selected genre
  const filteredMovies = useMemo(() => {
    if (!selectedGenre) return allMovies;

    return allMovies.filter((movie) =>
      movie.genres?.some((genre) => genre.id === selectedGenre)
    );
  }, [allMovies, selectedGenre]);

  if (loading) {
    return (
      <>
        <Header />
        <div className="loading-container">
          <div className="spinner"></div>
        </div>
      </>
    );
  }

  return (
    <>
      <Header />
      <main className="browse-page">
        <div className="browse-header">
          <h1 className="browse-title">Movies</h1>
          <div className="genre-filters">
            <button
              className={`genre-btn ${!selectedGenre ? 'active' : ''}`}
              onClick={() => setSelectedGenre(null)}
            >
              All
            </button>
            {genres.map((genre) => (
              <button
                key={genre.id}
                className={`genre-btn ${selectedGenre === genre.id ? 'active' : ''}`}
                onClick={() => setSelectedGenre(genre.id)}
              >
                {genre.name}
              </button>
            ))}
          </div>
        </div>

        {error && <div className="error-message">{error}</div>}

        <div className="browse-content">
          {filteredMovies.length > 0 ? (
            <div className="content-grid">
              {filteredMovies.map((movie) => (
                <MovieCard key={movie.id} item={movie} type="movie" />
              ))}
            </div>
          ) : (
            <div className="no-content">No movies found</div>
          )}
        </div>
      </main>
    </>
  );
};

export default Movies;
