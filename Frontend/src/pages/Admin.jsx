import { useState, useEffect } from 'react';
import Header from '../components/Header';
import { movieService } from '../services/movieService';
import { seriesService } from '../services/seriesService';
import { genreService } from '../services/genreService';
import './Admin.css';

const Admin = () => {
  const [activeTab, setActiveTab] = useState('movies');
  const [genres, setGenres] = useState([]);
  const [movies, setMovies] = useState([]);
  const [series, setSeries] = useState([]);
  const [editingItem, setEditingItem] = useState(null);
  const [message, setMessage] = useState('');

  const [movieForm, setMovieForm] = useState({
    title: '',
    description: '',
    releaseYear: '',
    imageUrl: '',
    genreIds: []
  });

  const [seriesForm, setSeriesForm] = useState({
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
    loadSeries();
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
      const response = await movieService.getAll();
      console.log('Movies loaded from API:', response.data);
      setMovies(response.data || []);
    } catch (error) {
      console.error('Error loading movies:', error);
    }
  };

  const loadSeries = async () => {
    try {
      const response = await seriesService.getAll();
      setSeries(response.data || []);
    } catch (error) {
      console.error('Error loading series:', error);
    }
  };

  const handleMovieSubmit = async (e) => {
    e.preventDefault();
    try {
      const formData = {
        ...movieForm,
        releaseYear: movieForm.releaseYear ? parseInt(movieForm.releaseYear) : null,
        genreIds: movieForm.genreIds.map(id => parseInt(id))
      };

      if (editingItem) {
        await movieService.update(editingItem.id, formData);
        setMessage('Movie updated successfully!');
      } else {
        await movieService.create(formData);
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

  const handleSeriesSubmit = async (e) => {
    e.preventDefault();
    try {
      const formData = {
        ...seriesForm,
        releaseYear: seriesForm.releaseYear ? parseInt(seriesForm.releaseYear) : null,
        seasonsCount: seriesForm.seasonsCount ? parseInt(seriesForm.seasonsCount) : null,
        episodesCount: seriesForm.episodesCount ? parseInt(seriesForm.episodesCount) : null,
        genreIds: seriesForm.genreIds.map(id => parseInt(id))
      };

      if (editingItem) {
        await seriesService.update(editingItem.id, formData);
        setMessage('Series updated successfully!');
      } else {
        await seriesService.create(formData);
        setMessage('Series created successfully!');
      }

      resetSeriesForm();
      loadSeries();
      setTimeout(() => setMessage(''), 3000);
    } catch (error) {
      setMessage(`Error: ${error.response?.data?.message || error.message}`);
      setTimeout(() => setMessage(''), 5000);
    }
  };

  const handleDeleteMovie = async (id) => {
    if (!window.confirm('Are you sure you want to delete this movie?')) return;

    try {
      await movieService.delete(id);
      setMessage('Movie deleted successfully!');
      loadMovies();
      setTimeout(() => setMessage(''), 3000);
    } catch (error) {
      setMessage(`Error: ${error.response?.data?.message || error.message}`);
      setTimeout(() => setMessage(''), 5000);
    }
  };

  const handleDeleteSeries = async (id) => {
    if (!window.confirm('Are you sure you want to delete this series?')) return;

    try {
      await seriesService.delete(id);
      setMessage('Series deleted successfully!');
      loadSeries();
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

  const editSeries = (seriesItem) => {
    console.log('Editing series:', seriesItem);
    setActiveTab('series');
    setSeriesForm({
      title: seriesItem.title || '',
      description: seriesItem.description || '',
      releaseYear: seriesItem.releaseYear ? seriesItem.releaseYear.toString() : '',
      seasonsCount: seriesItem.seasonsCount ? seriesItem.seasonsCount.toString() : '',
      episodesCount: seriesItem.episodesCount ? seriesItem.episodesCount.toString() : '',
      imageUrl: seriesItem.imageUrl || '',
      genreIds: seriesItem.genres?.map(g => g.id) || []
    });
    setEditingItem(seriesItem);
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

  const resetSeriesForm = () => {
    setSeriesForm({
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
      setSeriesForm(prev => ({
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
            className={`tab-button ${activeTab === 'series' ? 'active' : ''}`}
            onClick={() => {
              setActiveTab('series');
              resetSeriesForm();
            }}
          >
            Manage Series
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

        {activeTab === 'series' && (
          <div className="admin-content">
            <div className="form-section">
              <h2>{editingItem ? 'Edit Series' : 'Add New Series'}</h2>
              <form onSubmit={handleSeriesSubmit} className="admin-form">
                <div className="form-group">
                  <label>Title *</label>
                  <input
                    type="text"
                    value={seriesForm.title}
                    onChange={(e) => setSeriesForm({ ...seriesForm, title: e.target.value })}
                    required
                  />
                </div>

                <div className="form-group">
                  <label>Description *</label>
                  <textarea
                    value={seriesForm.description}
                    onChange={(e) => setSeriesForm({ ...seriesForm, description: e.target.value })}
                    required
                    rows="4"
                  />
                </div>

                <div className="form-row">
                  <div className="form-group">
                    <label>Release Year</label>
                    <input
                      type="number"
                      value={seriesForm.releaseYear}
                      onChange={(e) => setSeriesForm({ ...seriesForm, releaseYear: e.target.value })}
                      min="1800"
                      max="2100"
                    />
                  </div>

                  <div className="form-group">
                    <label>Seasons Count</label>
                    <input
                      type="number"
                      value={seriesForm.seasonsCount}
                      onChange={(e) => setSeriesForm({ ...seriesForm, seasonsCount: e.target.value })}
                      min="1"
                      max="100"
                    />
                  </div>
                </div>

                <div className="form-group">
                  <label>Episodes Count</label>
                  <input
                    type="number"
                    value={seriesForm.episodesCount}
                    onChange={(e) => setSeriesForm({ ...seriesForm, episodesCount: e.target.value })}
                    min="1"
                    max="10000"
                  />
                </div>

                <div className="form-group">
                  <label>Image URL *</label>
                  <input
                    type="url"
                    value={seriesForm.imageUrl}
                    onChange={(e) => setSeriesForm({ ...seriesForm, imageUrl: e.target.value })}
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
                          checked={seriesForm.genreIds.includes(genre.id)}
                          onChange={() => handleGenreToggle(genre.id, false)}
                        />
                        {genre.name}
                      </label>
                    ))}
                  </div>
                </div>

                <div className="form-actions">
                  <button type="submit" className="btn-primary">
                    {editingItem ? 'Update Series' : 'Create Series'}
                  </button>
                  {editingItem && (
                    <button type="button" onClick={resetSeriesForm} className="btn-secondary">
                      Cancel
                    </button>
                  )}
                </div>
              </form>
            </div>

            <div className="list-section">
              <h2>All Series</h2>
              <div className="items-list">
                {series.map(seriesItem => (
                  <div key={seriesItem.id} className="item-card">
                    <img src={seriesItem.imageUrl} alt={seriesItem.title} />
                    <div className="item-info">
                      <h3>{seriesItem.title}</h3>
                      <p>{seriesItem.releaseYear} • {seriesItem.seasonsCount} seasons • {seriesItem.episodesCount} episodes</p>
                      <div className="item-genres">
                        {seriesItem.genres?.map(g => g.name).join(', ')}
                      </div>
                      <div className="item-actions">
                        <button onClick={() => editSeries(seriesItem)} className="btn-edit">
                          Edit
                        </button>
                        <button onClick={() => handleDeleteSeries(seriesItem.id)} className="btn-delete">
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
