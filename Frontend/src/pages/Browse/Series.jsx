import { useState, useEffect, useMemo } from 'react';
import { titleService } from '../../services/titleService';
import { genreService } from '../../services/genreService';
import Header from '../../components/Header';
import TitleCard from '../../components/TitleCard';
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
          <div className="genre-filter-dropdown">
            <label htmlFor="genre-select" className="filter-label">Genre:</label>
            <select
              id="genre-select"
              className="genre-select"
              value={selectedGenre || ''}
              onChange={(e) => setSelectedGenre(e.target.value ? parseInt(e.target.value) : null)}
            >
              <option value="">All Genres</option>
              {genres.map((genre) => (
                <option key={genre.id} value={genre.id}>
                  {genre.name}
                </option>
              ))}
            </select>
          </div>
        </div>

        {error && <div className="error-message">{error}</div>}

        <div className="browse-content">
          {filteredSeries.length > 0 ? (
            <div className="content-grid">
              {filteredSeries.map((s) => (
                <TitleCard key={s.id} item={s} type="series" />
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
