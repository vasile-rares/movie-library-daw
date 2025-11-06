import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { toWatchService } from '../services/toWatchService';
import Header from '../components/Header';
import './MyList.css';

const MyList = () => {
  const [list, setList] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    fetchMyList();
  }, []);

  const fetchMyList = async () => {
    try {
      setLoading(true);
      const response = await toWatchService.getMyList();
      setList(response.data || []);
    } catch (err) {
      setError('Failed to load your list');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleRemove = async (id) => {
    if (!confirm('Remove from your list?')) return;

    try {
      await toWatchService.removeFromList(id);
      setList(list.filter((item) => item.id !== id));
    } catch (err) {
      console.error(err);
      alert('Failed to remove from list');
    }
  };

  const handleItemClick = (item) => {
    if (item.movieId) {
      navigate(`/movie/${item.movieId}`);
    } else if (item.seriesId) {
      navigate(`/series/${item.seriesId}`);
    }
  };

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
      <main className="my-list-page">
        <div className="my-list-header">
          <h1 className="my-list-title">My List</h1>
          <p className="my-list-subtitle">
            {list.length} {list.length === 1 ? 'item' : 'items'} in your list
          </p>
        </div>

        {error && <div className="error-message">{error}</div>}

        <div className="my-list-content">
          {list.length > 0 ? (
            <div className="my-list-grid">
              {list.map((item) => {
                const title = item.movieTitle || item.seriesTitle;
                const imageUrl = item.movieImageUrl || item.seriesImageUrl;
                const imageSrc = imageUrl || `https://via.placeholder.com/300x450/141414/e50914?text=${encodeURIComponent(title)}`;

                return (
                  <div key={item.id} className="my-list-item">
                    <div
                      className="my-list-item-image"
                      onClick={() => handleItemClick(item)}
                    >
                      <img src={imageSrc} alt={title} />
                      <div className="my-list-item-overlay">
                        <h3>{title}</h3>
                        <p className="item-type">
                          {item.movieId ? 'Movie' : 'Series'}
                        </p>
                      </div>
                    </div>
                    <button
                      className="remove-btn"
                      onClick={() => handleRemove(item.id)}
                    >
                      Remove
                    </button>
                  </div>
                );
              })}
            </div>
          ) : (
            <div className="empty-list">
              <h2>Your list is empty</h2>
              <p>Start adding movies and series to your list!</p>
              <button className="browse-btn" onClick={() => navigate('/movies')}>
                Browse Movies
              </button>
            </div>
          )}
        </div>
      </main>
    </>
  );
};

export default MyList;
