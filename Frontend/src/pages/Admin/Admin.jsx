import { useState, useEffect } from 'react';
import Header from '../../components/Header';
import { titleService } from '../../services/titleService';
import { genreService } from '../../services/genreService';
import './Admin.css';

const Admin = () => {
  const [activeTab, setActiveTab] = useState('movies');
  const [genres, setGenres] = useState([]);
  const [movies, setMovies] = useState([]);
  const [shows, setShows] = useState([]);
  const [editingItem, setEditingItem] = useState(null);
  const [message, setMessage] = useState('');

  const [movieForm, setMovieForm] = useState({
    title: '',
    description: '',
    releaseYear: '',
    imageUrl: '',
    genreIds: []
  });

  const [showForm, setShowsForm] = useState({
    title: '',
    description: '',
    releaseYear: '',
    seasonsCount: '',
    episodesCount: '',
    imageUrl: '',
    genreIds: []
  });

  useEffect(() => {
    loadGenres();
    loadMovies();
    loadShows();
  }, []);

  const loadGenres = async () => {
    try {
      const response = await genreService.getAll();
      setGenres(response.data || []);
    } catch (error) {
      console.error('Error loading genres:', error);
    }
  };

  const loadMovies = async () => {
    try {
      const response = await titleService.getByType(0);
      console.log('Movies loaded from API:', response.data);
      setMovies(response.data || []);
    } catch (error) {
      console.error('Error loading movies:', error);
    }
  };

  const loadShows = async () => {
    try {
      const response = await titleService.getByType(1);
      setShows(response.data || []);
    } catch (error) {
      console.error('Error loading show:', error);
    }
  };

  const handleMovieSubmit = async (e) => {
    e.preventDefault();
    try {
      const formData = {
        ...movieForm,
        type: 0,
        releaseYear: movieForm.releaseYear ? parseInt(movieForm.releaseYear) : null,
        genreIds: movieForm.genreIds.map(id => parseInt(id))
      };

      if (editingItem) {
        await titleService.update(editingItem.id, formData);
        setMessage('Movie updated successfully!');
      } else {
        await titleService.create(formData);
        setMessage('Movie created successfully!');
      }

      resetMovieForm();
      loadMovies();
      setTimeout(() => setMessage(''), 3000);
    } catch (error) {
      setMessage(`Error: ${error.response?.data?.message || error.message}`);
      setTimeout(() => setMessage(''), 5000);
    }
  };

  const handleShowSubmit = async (e) => {
    e.preventDefault();
    try {
      const formData = {
        ...showForm,
        type: 1,
        releaseYear: showForm.releaseYear ? parseInt(showForm.releaseYear) : null,
        seasonsCount: showForm.seasonsCount ? parseInt(showForm.seasonsCount) : null,
        episodesCount: showForm.episodesCount ? parseInt(showForm.episodesCount) : null,
        genreIds: showForm.genreIds.map(id => parseInt(id))
      };

      if (editingItem) {
        await titleService.update(editingItem.id, formData);
        setMessage('Shows updated successfully!');
      } else {
        await titleService.create(formData);
        setMessage('Shows created successfully!');
      }

      resetShowsForm();
      loadShows();
      setTimeout(() => setMessage(''), 3000);
    } catch (error) {
      setMessage(`Error: ${error.response?.data?.message || error.message}`);
      setTimeout(() => setMessage(''), 5000);
    }
  };

  const handleDeleteMovie = async (id) => {
    if (!window.confirm('Are you sure you want to delete this movie?')) return;

    try {
      await titleService.delete(id);
      setMessage('Movie deleted successfully!');
      loadMovies();
      setTimeout(() => setMessage(''), 3000);
    } catch (error) {
      setMessage(`Error: ${error.response?.data?.message || error.message}`);
      setTimeout(() => setMessage(''), 5000);
    }
  };

  const handleDeleteShow = async (id) => {
    if (!window.confirm('Are you sure you want to delete this show?')) return;

    try {
      await titleService.delete(id);
      setMessage('Shows deleted successfully!');
      loadShows();
      setTimeout(() => setMessage(''), 3000);
    } catch (error) {
      setMessage(`Error: ${error.response?.data?.message || error.message}`);
      setTimeout(() => setMessage(''), 5000);
    }
  };

  const editMovie = (movie) => {
    console.log('Editing movie:', movie);
    setActiveTab('movies');
    setMovieForm({
      title: movie.title || '',
      description: movie.description || '',
      releaseYear: movie.releaseYear ? movie.releaseYear.toString() : '',
      imageUrl: movie.imageUrl || '',
      genreIds: movie.genres?.map(g => g.id) || []
    });
    setEditingItem(movie);
  };

  const editShow = (showItem) => {
    console.log('Editing show:', showItem);
    setActiveTab('shows');
    setShowsForm({
      title: showItem.title || '',
      description: showItem.description || '',
      releaseYear: showItem.releaseYear ? showItem.releaseYear.toString() : '',
      seasonsCount: showItem.seasonsCount ? showItem.seasonsCount.toString() : '',
      episodesCount: showItem.episodesCount ? showItem.episodesCount.toString() : '',
      imageUrl: showItem.imageUrl || '',
      genreIds: showItem.genres?.map(g => g.id) || []
    });
    setEditingItem(showItem);
  };

  const resetMovieForm = () => {
    setMovieForm({
      title: '',
      description: '',
      releaseYear: '',
      imageUrl: '',
      genreIds: []
    });
    setEditingItem(null);
  };

  const resetShowsForm = () => {
    setShowsForm({
      title: '',
      description: '',
      releaseYear: '',
      seasonsCount: '',
      episodesCount: '',
      imageUrl: '',
      genreIds: []
    });
    setEditingItem(null);
  };

  const handleGenreToggle = (genreId, isMovie) => {
    if (isMovie) {
      setMovieForm(prev => ({
        ...prev,
        genreIds: prev.genreIds.includes(genreId)
          ? prev.genreIds.filter(id => id !== genreId)
          : [...prev.genreIds, genreId]
      }));
    } else {
      setShowsForm(prev => ({
        ...prev,
        genreIds: prev.genreIds.includes(genreId)
          ? prev.genreIds.filter(id => id !== genreId)
          : [...prev.genreIds, genreId]
      }));
    }
  };

  return (
    <div className="admin-page">
      <Header />
      <div className="admin-container">
        <h1>Admin Dashboard</h1>

        {message && (
          <div className={`admin-message ${message.includes('Error') ? 'error' : 'success'}`}>
            {message}
          </div>
        )}

        <div className="admin-tabs">
          <button
            className={`tab-button ${activeTab === 'movies' ? 'active' : ''}`}
            onClick={() => {
              setActiveTab('movies');
              resetMovieForm();
            }}
          >
            Manage Movies
          </button>
          <button
            className={`tab-button ${activeTab === 'shows' ? 'active' : ''}`}
            onClick={() => {
              setActiveTab('shows');
              resetShowsForm();
            }}
          >
            Manage Shows
          </button>
        </div>

        {activeTab === 'movies' && (
          <div className="admin-content">
            <div className="form-section">
              <h2>{editingItem ? 'Edit Movie' : 'Add New Movie'}</h2>
              <form onSubmit={handleMovieSubmit} className="admin-form">
                <div className="form-group">
                  <label>Title *</label>
                  <input
                    type="text"
                    value={movieForm.title}
                    onChange={(e) => setMovieForm({ ...movieForm, title: e.target.value })}
                    required
                  />
                </div>

                <div className="form-group">
                  <label>Description *</label>
                  <textarea
                    value={movieForm.description}
                    onChange={(e) => setMovieForm({ ...movieForm, description: e.target.value })}
                    required
                    rows="4"
                  />
                </div>

                <div className="form-group">
                  <label>Release Year</label>
                  <input
                    type="number"
                    value={movieForm.releaseYear}
                    onChange={(e) => setMovieForm({ ...movieForm, releaseYear: e.target.value })}
                    min="1800"
                    max="2100"
                  />
                </div>

                <div className="form-group">
                  <label>Image URL *</label>
                  <input
                    type="url"
                    value={movieForm.imageUrl}
                    onChange={(e) => setMovieForm({ ...movieForm, imageUrl: e.target.value })}
                    required
                  />
                </div>

                <div className="form-group">
                  <label>Genres *</label>
                  <div className="genre-checkboxes">
                    {genres.map(genre => (
                      <label key={genre.id} className="checkbox-label">
                        <input
                          type="checkbox"
                          checked={movieForm.genreIds.includes(genre.id)}
                          onChange={() => handleGenreToggle(genre.id, true)}
                        />
                        {genre.name}
                      </label>
                    ))}
                  </div>
                </div>

                <div className="form-actions">
                  <button type="submit" className="btn-primary">
                    {editingItem ? 'Update Movie' : 'Create Movie'}
                  </button>
                  {editingItem && (
                    <button type="button" onClick={resetMovieForm} className="btn-secondary">
                      Cancel
                    </button>
                  )}
                </div>
              </form>
            </div>

            <div className="list-section">
              <h2>All Movies</h2>
              <div className="items-list">
                {movies.map(movie => (
                  <div key={movie.id} className="item-card">
                    <img src={movie.imageUrl} alt={movie.title} />
                    <div className="item-info">
                      <h3>{movie.title}</h3>
                      <p>{movie.releaseYear}</p>
                      <div className="item-genres">
                        {movie.genres?.map(g => g.name).join(', ')}
                      </div>
                      <div className="item-actions">
                        <button onClick={() => editMovie(movie)} className="btn-edit">
                          Edit
                        </button>
                        <button onClick={() => handleDeleteMovie(movie.id)} className="btn-delete">
                          Delete
                        </button>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          </div>
        )}

        {activeTab === 'shows' && (
          <div className="admin-content">
            <div className="form-section">
              <h2>{editingItem ? 'Edit Shows' : 'Add New Shows'}</h2>
              <form onSubmit={handleShowSubmit} className="admin-form">
                <div className="form-group">
                  <label>Title *</label>
                  <input
                    type="text"
                    value={showForm.title}
                    onChange={(e) => setShowsForm({ ...showForm, title: e.target.value })}
                    required
                  />
                </div>

                <div className="form-group">
                  <label>Description *</label>
                  <textarea
                    value={showForm.description}
                    onChange={(e) => setShowsForm({ ...showForm, description: e.target.value })}
                    required
                    rows="4"
                  />
                </div>

                <div className="form-row">
                  <div className="form-group">
                    <label>Release Year</label>
                    <input
                      type="number"
                      value={showForm.releaseYear}
                      onChange={(e) => setShowsForm({ ...showForm, releaseYear: e.target.value })}
                      min="1800"
                      max="2100"
                    />
                  </div>

                  <div className="form-group">
                    <label>Seasons Count</label>
                    <input
                      type="number"
                      value={showForm.seasonsCount}
                      onChange={(e) => setShowsForm({ ...showForm, seasonsCount: e.target.value })}
                      min="1"
                      max="100"
                    />
                  </div>
                </div>

                <div className="form-group">
                  <label>Episodes Count</label>
                  <input
                    type="number"
                    value={showForm.episodesCount}
                    onChange={(e) => setShowsForm({ ...showForm, episodesCount: e.target.value })}
                    min="1"
                    max="10000"
                  />
                </div>

                <div className="form-group">
                  <label>Image URL *</label>
                  <input
                    type="url"
                    value={showForm.imageUrl}
                    onChange={(e) => setShowsForm({ ...showForm, imageUrl: e.target.value })}
                    required
                  />
                </div>

                <div className="form-group">
                  <label>Genres *</label>
                  <div className="genre-checkboxes">
                    {genres.map(genre => (
                      <label key={genre.id} className="checkbox-label">
                        <input
                          type="checkbox"
                          checked={showForm.genreIds.includes(genre.id)}
                          onChange={() => handleGenreToggle(genre.id, false)}
                        />
                        {genre.name}
                      </label>
                    ))}
                  </div>
                </div>

                <div className="form-actions">
                  <button type="submit" className="btn-primary">
                    {editingItem ? 'Update Shows' : 'Create Shows'}
                  </button>
                  {editingItem && (
                    <button type="button" onClick={resetShowsForm} className="btn-secondary">
                      Cancel
                    </button>
                  )}
                </div>
              </form>
            </div>

            <div className="list-section">
              <h2>All Shows</h2>
              <div className="items-list">
                {shows.map(showItem => (
                  <div key={showItem.id} className="item-card">
                    <img src={showItem.imageUrl} alt={showItem.title} />
                    <div className="item-info">
                      <h3>{showItem.title}</h3>
                      <p>{showItem.releaseYear} • {showItem.seasonsCount} seasons • {showItem.episodesCount} episodes</p>
                      <div className="item-genres">
                        {showItem.genres?.map(g => g.name).join(', ')}
                      </div>
                      <div className="item-actions">
                        <button onClick={() => editShow(showItem)} className="btn-edit">
                          Edit
                        </button>
                        <button onClick={() => handleDeleteShow(showItem.id)} className="btn-delete">
                          Delete
                        </button>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default Admin;
