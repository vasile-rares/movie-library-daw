import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { titleService } from '../services/titleService';
import { ratingService } from '../services/ratingService';
import { myListService } from '../services/myListService';
import { useAuth } from '../context/AuthContext';
import Header from '../components/Header';
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
        ratingService.getByMovie(id),
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
        movieId: parseInt(id),
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
        <div className="detail-hero" style={{ backgroundImage: `url(${imageSrc})` }}>
          <div className="detail-hero-overlay">
            <div className="detail-hero-content">
              <h1 className="detail-title">{movie.title}</h1>
              <div className="detail-meta">
                <span className="detail-year">{movie.releaseYear}</span>
                <span className="detail-rating">★ {averageRating}/10</span>
                <span className="detail-count">({ratings.length} ratings)</span>
              </div>
            </div>
          </div>
        </div>

        <div className="detail-content">
          <div className="detail-main">
            <section className="detail-section">
              <h2>Overview</h2>
              <p className="detail-description">{movie.description}</p>
              {movie.genres && movie.genres.length > 0 && (
                <div className="detail-genres">
                  <strong>Genres:</strong>
                  {movie.genres.map((genre) => (
                    <span key={genre.id} className="genre-tag">
                      {genre.name}
                    </span>
                  ))}
                </div>
              )}
              <button className="add-to-list-detail-btn" onClick={handleAddToList}>
                + Add to My List
              </button>
            </section>

            <section className="detail-section">
              <h2>Your Rating</h2>
              <form onSubmit={handleSubmitRating} className="rating-form">
                <div className="rating-input-group">
                  <label htmlFor="score">Score (1-10):</label>
                  <input
                    type="number"
                    id="score"
                    min="1"
                    max="10"
                    value={newRating.score}
                    onChange={(e) => setNewRating({ ...newRating, score: e.target.value })}
                    required
                  />
                </div>
                <div className="rating-input-group">
                  <label htmlFor="comment">Comment (optional):</label>
                  <textarea
                    id="comment"
                    rows="4"
                    maxLength="300"
                    value={newRating.comment}
                    onChange={(e) => setNewRating({ ...newRating, comment: e.target.value })}
                    placeholder="Share your thoughts..."
                  />
                </div>
                <button type="submit" className="submit-rating-btn">
                  {userRating ? 'Update Rating' : 'Submit Rating'}
                </button>
              </form>
            </section>

            <section className="detail-section">
              <h2>User Reviews ({ratings.length})</h2>
              <div className="reviews-list">
                {ratings.length > 0 ? (
                  ratings.map((rating) => (
                    <div key={rating.id} className="review-card">
                      <div className="review-header">
                        <span className="review-author">{rating.username}</span>
                        <span className="review-score">★ {rating.score}/10</span>
                      </div>
                      {rating.comment && <p className="review-comment">{rating.comment}</p>}
                      <span className="review-date">
                        {new Date(rating.createdAt).toLocaleDateString()}
                      </span>
                    </div>
                  ))
                ) : (
                  <p className="no-reviews">No reviews yet. Be the first to rate this movie!</p>
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
