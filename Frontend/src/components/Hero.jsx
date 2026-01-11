import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "./Hero.css";

const Hero = ({ items, type = "movie" }) => {
  const navigate = useNavigate();
  const [currentIndex, setCurrentIndex] = useState(0);
  const [isTransitioning, setIsTransitioning] = useState(false);

  useEffect(() => {
    if (!items || items.length <= 1) return;

    const interval = setInterval(() => {
      nextSlide();
    }, 5000);

    return () => clearInterval(interval);
  }, [currentIndex, items]);

  if (!items || items.length === 0) return null;

  const currentItem = items[currentIndex];
  const imageSrc =
    currentItem.imageUrl ||
    `https://via.placeholder.com/1920x800/141414/e50914?text=${encodeURIComponent(
      currentItem.title
    )}`;

  const handlePlayClick = () => {
    navigate(`/${type}/${currentItem.id}`);
  };

  const nextSlide = () => {
    setIsTransitioning(true);
    setTimeout(() => {
      setCurrentIndex((prev) => (prev + 1) % items.length);
      setIsTransitioning(false);
    }, 300);
  };

  const goToSlide = (index) => {
    if (index === currentIndex) return;
    setIsTransitioning(true);
    setTimeout(() => {
      setCurrentIndex(index);
      setIsTransitioning(false);
    }, 300);
  };

  return (
    <div className="hero-slideshow">
      <div
        className={`hero ${isTransitioning ? "hero-transitioning" : ""}`}
        style={{ backgroundImage: `url(${imageSrc})` }}
      >
        <div className="hero-overlay">
          <div className="hero-content">
            <h1 className="hero-title">{currentItem.title}</h1>
            <p className="hero-description">{currentItem.description}</p>
            <div className="hero-info">
              <span className="hero-year">
                Release Year: {currentItem.releaseYear}
              </span>
              {currentItem.seasonsCount && (
                <span className="hero-seasons">
                  {currentItem.seasonsCount} Seasons
                </span>
              )}
            </div>
            <div className="hero-buttons">
              <button
                className="hero-btn hero-btn-play"
                onClick={handlePlayClick}
              >
                <span>▶</span> View Details
              </button>
            </div>
          </div>
        </div>
      </div>

      {/* Dots indicator - only show if more than 1 item */}
      {items.length > 1 && (
        <div className="hero-dots">
          {items.map((_, index) => (
            <button
              key={index}
              className={`hero-dot ${index === currentIndex ? "active" : ""}`}
              onClick={() => goToSlide(index)}
              aria-label={`Go to slide ${index + 1}`}
            />
          ))}
        </div>
      )}
    </div>
  );
};

export default Hero;
