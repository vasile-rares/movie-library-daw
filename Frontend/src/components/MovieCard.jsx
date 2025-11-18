import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { myListService } from '../services/myListService';
import './MovieCard.css';

const MovieCard = ({ item, type = 'movie', onAddToList, isInList = false }) => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const handleClick = () => {
    navigate(`/${type}/${item.id}`);
  };

  const handleAddToList = async (e) => {
    e.stopPropagation();
    setLoading(true);
    try {
      await myListService.addToList({ titleId: item.id, status: 0 });
      if (onAddToList) onAddToList();
    } catch (error) {
      console.error('Error adding to list:', error);
      alert('Failed to add to list');
    } finally {
      setLoading(false);
    }
  };

  const imageSrc = item.imageUrl || `https://via.placeholder.com/300x450/141414/e50914?text=${encodeURIComponent(item.title)}`;

  return (
    <div className="movie-card">
      <div className="card-poster" onClick={handleClick}>
        <img src={imageSrc} alt={item.title} loading="lazy" />
        {!isInList && (
          <button
            className="quick-add"
            onClick={handleAddToList}
            disabled={loading}
            title="Add to My List"
          >
            <svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor">
              <path d="M8 2v12M2 8h12" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/>
            </svg>
          </button>
        )}
      </div>
      <div className="card-info">
        <h3 className="card-title" onClick={handleClick} title={item.title}>
          {item.title}
        </h3>
        <div className="card-meta">
          <span className="meta-year">{item.releaseYear}</span>
          {item.seasonsCount && (
            <span className="meta-seasons">• {item.seasonsCount} Seasons</span>
          )}
        </div>
      </div>
    </div>
  );
};

export default MovieCard;
