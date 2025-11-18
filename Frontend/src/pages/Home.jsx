import { useState, useEffect } from 'react';
import { titleService } from '../services/titleService';
import Header from '../components/Header';
import Hero from '../components/Hero';
import TitleCard from '../components/TitleCard';
import './Home.css';

const Home = () => {
  const [movies, setMovies] = useState([]);
  const [series, setSeries] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchContent();
  }, []);

  const fetchContent = async () => {
    try {
      setLoading(true);
      const [moviesRes, seriesRes] = await Promise.all([
        titleService.getByType(0), // TitleType.Movie = 0
        titleService.getByType(1), // TitleType.Series = 1
      ]);
      setMovies(moviesRes.data || []);
      setSeries(seriesRes.data || []);
    } catch (err) {
      setError('Failed to load content');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const featuredContent = [...movies, ...series][0];

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

  if (error) {
    return (
      <>
        <Header />
        <div className="error-container">{error}</div>
      </>
    );
  }

  return (
    <>
      <Header />
      <main className="home">
        {featuredContent && <Hero item={featuredContent} type={featuredContent.seasonsCount ? 'series' : 'movie'} />}

        {movies.length > 0 && (
          <section className="content-section">
            <h2 className="section-title">Popular Movies</h2>
            <div className="content-grid">
              {movies.slice(0, 12).map((movie) => (
                <TitleCard key={movie.id} item={movie} type="movie" />
              ))}
            </div>
          </section>
        )}

        {series.length > 0 && (
          <section className="content-section">
            <h2 className="section-title">Popular Series</h2>
            <div className="content-grid">
              {series.slice(0, 12).map((s) => (
                <TitleCard key={s.id} item={s} type="series" />
              ))}
            </div>
          </section>
        )}
      </main>
    </>
  );
};

export default Home;
