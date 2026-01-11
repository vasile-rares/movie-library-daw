import { useState, useEffect, useRef } from 'react';
import { titleService } from '../../services/titleService';
import { myListService } from '../../services/myListService';
import Header from '../../components/Header';
import Hero from '../../components/Hero';
import TitleCard from '../../components/TitleCard';
import './Home.css';

const Home = () => {
  const [movies, setMovies] = useState([]);
  const [shows, setShows] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [myListByTitleId, setMyListByTitleId] = useState({});
  const moviesScrollRef = useRef(null);
  const showsScrollRef = useRef(null);

  useEffect(() => {
    fetchContent();
    fetchMyList();
  }, []);

  const fetchContent = async () => {
    try {
      setLoading(true);
      const [moviesRes, showsRes] = await Promise.all([
        titleService.getByType(0), // TitleType.Movie = 0
        titleService.getByType(1), // TitleType.Show = 1
      ]);
      setMovies(moviesRes.data || []);
      setShows(showsRes.data || []);
    } catch (err) {
      setError('Failed to load content');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const fetchMyList = async () => {
    try {
      const response = await myListService.getMyList();
      const map = (response.data || []).reduce((acc, item) => {
        acc[item.titleId] = item.id;
        return acc;
      }, {});
      setMyListByTitleId(map);
    } catch (err) {
      console.error('Failed to load My List', err);
    }
  };

  const handleAddedToList = (createdItem) => {
    if (!createdItem) return;
    setMyListByTitleId((prev) => {
      if (prev[createdItem.titleId]) return prev;
      return { ...prev, [createdItem.titleId]: createdItem.id };
    });
  };

  const handleRemovedFromList = (titleId) => {
    setMyListByTitleId((prev) => {
      if (!prev[titleId]) return prev;
      const next = { ...prev };
      delete next[titleId];
      return next;
    });
  };

  const scroll = (ref, direction) => {
    if (ref.current) {
      const scrollAmount = 600;
      const newScrollPosition = ref.current.scrollLeft + (direction === 'left' ? -scrollAmount : scrollAmount);
      ref.current.scrollTo({
        left: newScrollPosition,
        behavior: 'smooth'
      });
    }
  };

  // Get top 5 items for the slideshow
  const featuredContent = [...movies, ...shows].slice(0, 5);

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
        {featuredContent.length > 0 && <Hero items={featuredContent} type="movie" />}

        {movies.length > 0 && (
          <section className="content-section">
            <h2 className="section-title">Popular Movies</h2>
            <div className="carousel-container">
              <button
                className="carousel-arrow carousel-arrow-left"
                onClick={() => scroll(moviesScrollRef, 'left')}
                aria-label="Scroll left"
              >
                <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                  <polyline points="15 18 9 12 15 6"></polyline>
                </svg>
              </button>
              <div className="content-carousel" ref={moviesScrollRef}>
                {movies.map((movie) => (
                  <TitleCard
                    key={movie.id}
                    item={movie}
                    type="movie"
                    isInList={!!myListByTitleId[movie.id]}
                    listItemId={myListByTitleId[movie.id]}
                    onAddToList={handleAddedToList}
                    onRemoveFromList={handleRemovedFromList}
                  />
                ))}
              </div>
              <button
                className="carousel-arrow carousel-arrow-right"
                onClick={() => scroll(moviesScrollRef, 'right')}
                aria-label="Scroll right"
              >
                <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                  <polyline points="9 18 15 12 9 6"></polyline>
                </svg>
              </button>
            </div>
          </section>
        )}

        {shows.length > 0 && (
          <section className="content-section">
            <h2 className="section-title">Popular Shows</h2>
            <div className="carousel-container">
              <button
                className="carousel-arrow carousel-arrow-left"
                onClick={() => scroll(showsScrollRef, 'left')}
                aria-label="Scroll left"
              >
                <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                  <polyline points="15 18 9 12 15 6"></polyline>
                </svg>
              </button>
              <div className="content-carousel" ref={showsScrollRef}>
                {shows.map((show) => (
                  <TitleCard
                    key={show.id}
                    item={show}
                    type="show"
                    isInList={!!myListByTitleId[show.id]}
                    listItemId={myListByTitleId[show.id]}
                    onAddToList={handleAddedToList}
                    onRemoveFromList={handleRemovedFromList}
                  />
                ))}
              </div>
              <button
                className="carousel-arrow carousel-arrow-right"
                onClick={() => scroll(showsScrollRef, 'right')}
                aria-label="Scroll right"
              >
                <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                  <polyline points="9 18 15 12 9 6"></polyline>
                </svg>
              </button>
            </div>
          </section>
        )}
      </main>
    </>
  );
};

export default Home;
