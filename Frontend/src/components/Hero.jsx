import { useNavigate } from 'react-router-dom';
import './Hero.css';

const Hero = ({ item, type = 'movie' }) => {
  const navigate = useNavigate();

  if (!item) return null;

  const imageSrc = item.imageUrl || `https://via.placeholder.com/1920x800/141414/e50914?text=${encodeURIComponent(item.title)}`;

  const handlePlayClick = () => {
    navigate(`/${type}/${item.id}`);
  };

  return (
    <div className="hero" style={{ backgroundImage: `url(${imageSrc})` }}>
      <div className="hero-overlay">
        <div className="hero-content">
          <h1 className="hero-title">{item.title}</h1>
          <p className="hero-description">{item.description}</p>
          <div className="hero-info">
            <span className="hero-year">{item.releaseYear}</span>
            {item.seasonsCount && (
              <span className="hero-seasons">{item.seasonsCount} Seasons</span>
            )}
          </div>
          <div className="hero-buttons">
            <button className="hero-btn hero-btn-play" onClick={handlePlayClick}>
              <span>▶</span> View Details
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Hero;
