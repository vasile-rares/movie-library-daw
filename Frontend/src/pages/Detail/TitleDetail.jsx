import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { titleService } from "../../services/titleService";
import { ratingService } from "../../services/ratingService";
import { myListService, WatchStatus } from "../../services/myListService";
import { useAuth } from "../../context/AuthContext";
import Header from "../../components/Header";
import "./TitleDetail.css";

const TitleDetail = ({ titleType }) => {
  const { id } = useParams();
  const { user } = useAuth();
  const [title, setTitle] = useState(null);
  const [ratings, setRatings] = useState([]);
  const [userRating, setUserRating] = useState(null);
  const [newRating, setNewRating] = useState({ score: 5, comment: "" });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [myListItem, setMyListItem] = useState(null);

  const isMovie = titleType === "movie";
  const typeLabel = isMovie ? "Movie" : "Show";

  const statusLabels = {
    [WatchStatus.PlanToWatch]: "Plan to Watch",
    [WatchStatus.Watching]: "Watching",
    [WatchStatus.Completed]: "Completed",
    [WatchStatus.OnHold]: "On Hold",
    [WatchStatus.Dropped]: "Dropped",
  };

  const fetchTitleDetails = async () => {
    try {
      setLoading(true);
      const [titleRes, ratingsRes, myListRes] = await Promise.all([
        titleService.getById(id),
        ratingService.getByTitle(id),
        myListService.getMyList(),
      ]);

      setTitle(titleRes.data);
      setRatings(ratingsRes.data || []);

      const existingRating = (ratingsRes.data || []).find(
        (r) => r.userId === user?.userId
      );
      setUserRating(existingRating);
      if (existingRating) {
        setNewRating({
          score: existingRating.score,
          comment: existingRating.comment || "",
        });
      }

      const listItem = (myListRes.data || []).find(
        (item) => item.titleId === parseInt(id)
      );
      setMyListItem(listItem || null);
    } catch (err) {
      setError(`Failed to load ${typeLabel.toLowerCase()} details`);
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTitleDetails();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);

  const handleAddToList = async () => {
    try {
      const response = await myListService.addToList({
        titleId: parseInt(id),
        status: 0,
      });
      setMyListItem(response.data);
      alert("Added to My List!");
    } catch (err) {
      console.error(err);
      alert("Failed to add to list");
    }
  };

  const handleRemoveFromList = async () => {
    if (!myListItem) return;
    if (!confirm("Remove from your list?")) return;

    try {
      await myListService.removeFromList(myListItem.id);
      setMyListItem(null);
      alert("Removed from My List!");
    } catch (err) {
      console.error(err);
      alert("Failed to remove from list");
    }
  };

  const handleStatusChange = async (e) => {
    if (!myListItem) return;
    const newStatus = parseInt(e.target.value);

    try {
      await myListService.updateStatus(myListItem.id, newStatus);
      setMyListItem({ ...myListItem, status: newStatus });
    } catch (err) {
      console.error(err);
      alert("Failed to update status");
    }
  };

  const handleSubmitRating = async (e) => {
    e.preventDefault();
    try {
      const ratingData = {
        userId: user.userId,
        titleId: parseInt(id),
        score: parseInt(newRating.score),
        comment: newRating.comment,
      };

      if (userRating) {
        await ratingService.update(userRating.id, ratingData);
      } else {
        await ratingService.create(ratingData);
      }

      await fetchTitleDetails();
      alert("Rating submitted!");
    } catch (err) {
      console.error(err);
      alert("Failed to submit rating");
    }
  };

  const handleDeleteRating = async () => {
    if (!userRating) return;
    if (!confirm("Are you sure you want to delete your rating?")) return;

    try {
      await ratingService.delete(userRating.id);
      setUserRating(null);
      setNewRating({ score: 5, comment: "" });
      await fetchTitleDetails();
      alert("Rating deleted!");
    } catch (err) {
      console.error(err);
      alert("Failed to delete rating");
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

  if (error || !title) {
    return (
      <>
        <Header />
        <div className="error-container">
          {error || `${typeLabel} not found`}
        </div>
      </>
    );
  }

  const averageRating =
    ratings.length > 0
      ? (ratings.reduce((sum, r) => sum + r.score, 0) / ratings.length).toFixed(
          1
        )
      : "N/A";

  const imageSrc =
    title.imageUrl ||
    `https://via.placeholder.com/1920x800/141414/e50914?text=${encodeURIComponent(
      title.title
    )}`;

  return (
    <>
      <Header />
      <main className="detail-page">
        <div className="detail-container">
          <aside className="detail-sidebar">
            <div className="poster-container">
              <img src={imageSrc} alt={title.title} className="detail-poster" />
              {!myListItem ? (
                <button className="add-list-btn" onClick={handleAddToList}>
                  + Add to My List
                </button>
              ) : (
                <div className="list-controls">
                  <select
                    className="status-dropdown"
                    value={myListItem.status}
                    onChange={handleStatusChange}
                  >
                    {Object.entries(statusLabels).map(([value, label]) => (
                      <option key={value} value={value}>
                        {label}
                      </option>
                    ))}
                  </select>
                  <button
                    className="remove-list-btn"
                    onClick={handleRemoveFromList}
                  >
                    Remove from List
                  </button>
                </div>
              )}
            </div>

            <div className="info-panel">
              <h3 className="panel-heading">Information</h3>
              <div className="info-row">
                <span className="info-label">Year</span>
                <span className="info-value">{title.releaseYear}</span>
              </div>
              {!isMovie && title.seasonsCount && (
                <div className="info-row">
                  <span className="info-label">Seasons</span>
                  <span className="info-value">{title.seasonsCount}</span>
                </div>
              )}
              {!isMovie && title.episodesCount && (
                <div className="info-row">
                  <span className="info-label">Episodes</span>
                  <span className="info-value">{title.episodesCount}</span>
                </div>
              )}
              <div className="info-row">
                <span className="info-label">Rating</span>
                <span className="info-value">★ {averageRating}/10</span>
              </div>
              <div className="info-row">
                <span className="info-label">Reviews</span>
                <span className="info-value">{ratings.length}</span>
              </div>
              {title.genres && title.genres.length > 0 && (
                <div className="info-row genres-row">
                  <span className="info-label">Genres</span>
                  <div className="genres-list">
                    {title.genres.map((genre) => (
                      <span key={genre.id} className="genre-badge">
                        {genre.name}
                      </span>
                    ))}
                  </div>
                </div>
              )}
            </div>
          </aside>

          <div className="detail-main">
            <div className="detail-header">
              <span className="detail-type-badge">{typeLabel}</span>
              <h1 className="detail-heading">{title.title}</h1>
              <div className="detail-meta">
                <span className="detail-meta-item">{title.releaseYear}</span>
                <span className="detail-meta-separator"></span>
                <span className="detail-meta-item rating">
                  ★ {averageRating}/10
                </span>
                <span className="detail-meta-separator"></span>
                <span className="detail-meta-item">
                  {ratings.length} {ratings.length === 1 ? "review" : "reviews"}
                </span>
              </div>
            </div>

            <section className="content-block">
              <h2 className="block-title">Synopsis</h2>
              <p className="synopsis-text">{title.description}</p>
            </section>

            <section className="content-block">
              <h2 className="block-title">Your Rating</h2>
              <form onSubmit={handleSubmitRating} className="rating-form">
                <div className="form-row">
                  <label htmlFor="score">Score (1-10)</label>
                  <input
                    type="number"
                    id="score"
                    min="1"
                    max="10"
                    value={newRating.score}
                    onChange={(e) =>
                      setNewRating({ ...newRating, score: e.target.value })
                    }
                    className="score-input"
                    required
                  />
                </div>
                <div className="form-row">
                  <label htmlFor="comment">Comment</label>
                  <textarea
                    id="comment"
                    rows="4"
                    maxLength="300"
                    value={newRating.comment}
                    onChange={(e) =>
                      setNewRating({ ...newRating, comment: e.target.value })
                    }
                    placeholder="Share your thoughts..."
                    className="comment-input"
                  />
                </div>
                <div className="rating-actions">
                  <button type="submit" className="submit-btn">
                    {userRating ? "Update Rating" : "Submit Rating"}
                  </button>
                  {userRating && (
                    <button
                      type="button"
                      className="delete-rating-btn"
                      onClick={handleDeleteRating}
                    >
                      Delete Rating
                    </button>
                  )}
                </div>
              </form>
            </section>

            <section className="content-block">
              <h2 className="block-title">Reviews ({ratings.length})</h2>
              <div className="reviews-container">
                {ratings.length > 0 ? (
                  ratings.map((rating) => (
                    <div key={rating.id} className="review-item">
                      <div className="review-top">
                        <span className="reviewer-name">{rating.username}</span>
                        <span className="review-rating">
                          ★ {rating.score}/10
                        </span>
                      </div>
                      {rating.comment && (
                        <p className="review-text">{rating.comment}</p>
                      )}
                      <span className="review-timestamp">
                        {new Date(rating.createdAt).toLocaleDateString()}
                      </span>
                    </div>
                  ))
                ) : (
                  <p className="no-reviews-msg">
                    No reviews yet. Be the first to rate this{" "}
                    {typeLabel.toLowerCase()}!
                  </p>
                )}
              </div>
            </section>
          </div>
        </div>
      </main>
    </>
  );
};

export default TitleDetail;
