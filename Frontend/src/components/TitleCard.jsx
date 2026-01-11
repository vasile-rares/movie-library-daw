import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { myListService } from "../services/myListService";
import "./TitleCard.css";

const TitleCard = ({
  item,
  type = "movie",
  onAddToList,
  onRemoveFromList,
  isInList = false,
  listItemId = null,
}) => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const handleClick = () => {
    navigate(`/${type}/${item.id}`);
  };

  const handleAddToList = async (e) => {
    e.stopPropagation();
    setLoading(true);
    try {
      const response = await myListService.addToList({
        titleId: item.id,
        status: 0,
      });
      if (onAddToList) onAddToList(response.data);
    } catch (error) {
      console.error("Error adding to list:", error);
      alert("Failed to add to list");
    } finally {
      setLoading(false);
    }
  };

  const handleRemoveFromList = async (e) => {
    e.stopPropagation();
    if (!listItemId) return;
    setLoading(true);
    try {
      await myListService.removeFromList(listItemId);
      if (onRemoveFromList) onRemoveFromList(item.id);
    } catch (error) {
      console.error("Error removing from list:", error);
      alert("Failed to remove from list");
    } finally {
      setLoading(false);
    }
  };

  const imageSrc =
    item.imageUrl ||
    `https://via.placeholder.com/300x450/141414/e50914?text=${encodeURIComponent(
      item.title
    )}`;

  return (
    <div className="title-card">
      <div className="card-poster" onClick={handleClick}>
        <img src={imageSrc} alt={item.title} loading="lazy" />
        <button
          className="quick-add"
          onClick={isInList ? handleRemoveFromList : handleAddToList}
          disabled={loading}
          title={isInList ? "Remove from My List" : "Add to My List"}
          aria-label={isInList ? "Remove from My List" : "Add to My List"}
        >
          {isInList ? (
            <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
              <path
                d="M3.5 8.5l3 3 6-6"
                stroke="currentColor"
                strokeWidth="2"
                strokeLinecap="round"
                strokeLinejoin="round"
              />
            </svg>
          ) : (
            <svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor">
              <path
                d="M8 2v12M2 8h12"
                stroke="currentColor"
                strokeWidth="2"
                strokeLinecap="round"
              />
            </svg>
          )}
        </button>
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

export default TitleCard;
