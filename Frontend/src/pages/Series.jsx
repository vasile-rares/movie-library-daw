import { useState, useEffect, useMemo } from 'react';
import { titleService } from '../services/titleService';
import { genreService } from '../services/genreService';
import Header from '../components/Header';
import MovieCard from '../components/MovieCard';
import './Browse.css';

const Series = () => {
  const [allSeries, setAllSeries] = useState([]);
  const [genres, setGenres] = useState([]);
  const [selectedGenre, setSelectedGenre] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const [genresRes, seriesRes] = await Promise.all([
        genreService.getAll(),
        titleService.getByType(1)
      ]);
      setGenres(genresRes.data || []);
      setAllSeries(seriesRes.data || []);
    } catch (err) {
      setError('Failed to load series');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  // Filter series on the client side based on selected genre
  const filteredSeries = useMemo(() => {
    if (!selectedGenre) return allSeries;

    return allSeries.filter((series) =>
      series.genres?.some((genre) => genre.id === selectedGenre)
    );
  }, [allSeries, selectedGenre]);

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
          <h1 className="browse-title">Series</h1>
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
          {filteredSeries.length > 0 ? (
            <div className="content-grid">
              {filteredSeries.map((s) => (
                <MovieCard key={s.id} item={s} type="series" />
              ))}
            </div>
          ) : (
            <div className="no-content">No series found</div>
          )}
        </div>
      </main>
    </>
  );
};

export default Series;
