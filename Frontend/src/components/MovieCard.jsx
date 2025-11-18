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
    <div className="movie-card" onClick={handleClick}>
      <div className="movie-card-image">
        <img src={imageSrc} alt={item.title} />
        <div className="movie-card-overlay">
          <div className="overlay-content">
            <h3>{item.title}</h3>
            <div className="overlay-info">
              <span className="year">{item.releaseYear}</span>
              {item.seasonsCount && (
                <span className="seasons">{item.seasonsCount} Seasons</span>
              )}
            </div>
            <p className="description">{item.description}</p>
            {!isInList && (
              <button
                className="add-to-list-btn"
                onClick={handleAddToList}
                disabled={loading}
              >
                {loading ? '...' : '+ My List'}
              </button>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default MovieCard;
