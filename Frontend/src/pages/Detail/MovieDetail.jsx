import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { titleService } from '../../services/titleService';
import { ratingService } from '../../services/ratingService';
import { myListService } from '../../services/myListService';
import { useAuth } from '../../context/AuthContext';
import Header from '../../components/Header';
import './Detail.css';

const MovieDetail = () => {
  const { id } = useParams();
  const { user } = useAuth();
  const navigate = useNavigate();
  const [movie, setMovie] = useState(null);
  const [ratings, setRatings] = useState([]);
  const [userRating, setUserRating] = useState(null);
  const [newRating, setNewRating] = useState({ score: 5, comment: '' });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchMovieDetails();
  }, [id]);

  const fetchMovieDetails = async () => {
    try {
      setLoading(true);
      const [movieRes, ratingsRes] = await Promise.all([
        titleService.getById(id),
        ratingService.getByTitle(id),
      ]);

      setMovie(movieRes.data);
      setRatings(ratingsRes.data || []);

      const existingRating = (ratingsRes.data || []).find(r => r.userId === user?.userId);
      setUserRating(existingRating);
      if (existingRating) {
        setNewRating({ score: existingRating.score, comment: existingRating.comment || '' });
      }
    } catch (err) {
      setError('Failed to load movie details');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleAddToList = async () => {
    try {
      await myListService.addToList({ titleId: parseInt(id), status: 0 });
      alert('Added to My List!');
    } catch (err) {
      console.error(err);
      alert('Failed to add to list');
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

      await fetchMovieDetails();
      alert('Rating submitted!');
    } catch (err) {
      console.error(err);
      alert('Failed to submit rating');
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

  if (error || !movie) {
    return (
      <>
        <Header />
        <div className="error-container">{error || 'Movie not found'}</div>
      </>
    );
  }

  const averageRating = ratings.length > 0
    ? (ratings.reduce((sum, r) => sum + r.score, 0) / ratings.length).toFixed(1)
    : 'N/A';

  const imageSrc = movie.imageUrl || `https://via.placeholder.com/1920x800/141414/e50914?text=${encodeURIComponent(movie.title)}`;

  return (
    <>
      <Header />
      <main className="detail-page">
        <div className="detail-container">
          <aside className="detail-sidebar">
            <div className="poster-container">
              <img src={imageSrc} alt={movie.title} className="detail-poster" />
              <button className="add-list-btn" onClick={handleAddToList}>
                + Add to My List
              </button>
            </div>

            <div className="info-panel">
              <h3 className="panel-heading">Information</h3>
              <div className="info-row">
                <span className="info-label">Year</span>
                <span className="info-value">{movie.releaseYear}</span>
              </div>
              <div className="info-row">
                <span className="info-label">Rating</span>
                <span className="info-value">★ {averageRating}/10</span>
              </div>
              <div className="info-row">
                <span className="info-label">Reviews</span>
                <span className="info-value">{ratings.length}</span>
              </div>
              {movie.genres && movie.genres.length > 0 && (
                <div className="info-row genres-row">
                  <span className="info-label">Genres</span>
                  <div className="genres-list">
                    {movie.genres.map((genre) => (
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
              <span className="detail-type-badge">🎬 Movie</span>
              <h1 className="detail-heading">{movie.title}</h1>
              <div className="detail-meta">
                <span className="detail-meta-item">{movie.releaseYear}</span>
                <span className="detail-meta-separator"></span>
                <span className="detail-meta-item rating">★ {averageRating}/10</span>
                <span className="detail-meta-separator"></span>
                <span className="detail-meta-item">{ratings.length} {ratings.length === 1 ? 'review' : 'reviews'}</span>
              </div>
            </div>

            <section className="content-block">
              <h2 className="block-title">📖 Synopsis</h2>
              <p className="synopsis-text">{movie.description}</p>
            </section>

            <section className="content-block">
              <h2 className="block-title">⭐ Your Rating</h2>
              <form onSubmit={handleSubmitRating} className="rating-form">
                <div className="form-row">
                  <label htmlFor="score">Score (1-10)</label>
                  <input
                    type="number"
                    id="score"
                    min="1"
                    max="10"
                    value={newRating.score}
                    onChange={(e) => setNewRating({ ...newRating, score: e.target.value })}
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
                    onChange={(e) => setNewRating({ ...newRating, comment: e.target.value })}
                    placeholder="Share your thoughts..."
                    className="comment-input"
                  />
                </div>
                <button type="submit" className="submit-btn">
                  {userRating ? 'Update Rating' : 'Submit Rating'}
                </button>
              </form>
            </section>

            <section className="content-block">
              <h2 className="block-title">💬 Reviews ({ratings.length})</h2>
              <div className="reviews-container">
                {ratings.length > 0 ? (
                  ratings.map((rating) => (
                    <div key={rating.id} className="review-item">
                      <div className="review-top">
                        <span className="reviewer-name">{rating.username}</span>
                        <span className="review-rating">★ {rating.score}/10</span>
                      </div>
                      {rating.comment && <p className="review-text">{rating.comment}</p>}
                      <span className="review-timestamp">
                        {new Date(rating.createdAt).toLocaleDateString()}
                      </span>
                    </div>
                  ))
                ) : (
                  <p className="no-reviews-msg">No reviews yet. Be the first to rate this movie!</p>
                )}
              </div>
            </section>
          </div>
        </div>
      </main>
    </>
  );
};

export default MovieDetail;
