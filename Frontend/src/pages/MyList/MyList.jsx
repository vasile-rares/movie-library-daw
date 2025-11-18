import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { myListService } from '../../services/myListService';
import Header from '../../components/Header';
import './MyList.css';

const MyList = () => {
  const [list, setList] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [activeTab, setActiveTab] = useState('all');
  const navigate = useNavigate();

  useEffect(() => {
    fetchMyList();
  }, []);

  const fetchMyList = async () => {
    try {
      setLoading(true);
      const response = await myListService.getMyList();
      setList(response.data || []);
    } catch (err) {
      setError('Failed to load your list');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleRemove = async (id) => {
    if (!confirm('Remove from your list?')) return;

    try {
      await myListService.removeFromList(id);
      setList(list.filter((item) => item.id !== id));
    } catch (err) {
      console.error(err);
      alert('Failed to remove from list');
    }
  };

  const handleItemClick = (item) => {
    // titleType: 0 = Movie, 1 = Series
    if (item.titleType === 0) {
      navigate(`/movie/${item.titleId}`);
    } else if (item.titleType === 1) {
      navigate(`/show/${item.titleId}`);
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

  const statusNames = {
    0: 'Plan to Watch',
    1: 'Watching',
    2: 'Completed',
    3: 'On Hold',
    4: 'Dropped'
  };

  const filteredList = activeTab === 'all'
    ? list
    : list.filter(item => item.status === parseInt(activeTab));

  const getStatusCounts = () => {
    const counts = { all: list.length };
    [0, 1, 2, 3, 4].forEach(status => {
      counts[status] = list.filter(item => item.status === status).length;
    });
    return counts;
  };

  const counts = getStatusCounts();

  return (
    <>
      <Header />
      <main className="my-list-page">
        <div className="my-list-header">
          <h1 className="my-list-title">My List</h1>
        </div>

        <div className="my-list-tabs">
          <button
            className={`tab ${activeTab === 'all' ? 'active' : ''}`}
            onClick={() => setActiveTab('all')}
          >
            All ({counts.all})
          </button>
          <button
            className={`tab ${activeTab === '1' ? 'active' : ''}`}
            onClick={() => setActiveTab('1')}
          >
            Watching ({counts[1]})
          </button>
          <button
            className={`tab ${activeTab === '2' ? 'active' : ''}`}
            onClick={() => setActiveTab('2')}
          >
            Completed ({counts[2]})
          </button>
          <button
            className={`tab ${activeTab === '0' ? 'active' : ''}`}
            onClick={() => setActiveTab('0')}
          >
            Plan to Watch ({counts[0]})
          </button>
          <button
            className={`tab ${activeTab === '3' ? 'active' : ''}`}
            onClick={() => setActiveTab('3')}
          >
            On Hold ({counts[3]})
          </button>
          <button
            className={`tab ${activeTab === '4' ? 'active' : ''}`}
            onClick={() => setActiveTab('4')}
          >
            Dropped ({counts[4]})
          </button>
        </div>

        {error && <div className="error-message">{error}</div>}

        <div className="my-list-content">
          {filteredList.length > 0 ? (
            <div className="my-list-grid">
              {filteredList.map((item) => {
                const title = item.titleName;
                const imageUrl = item.titleImageUrl;
                const imageSrc = imageUrl || `https://via.placeholder.com/300x450/2e51a2/ffffff?text=${encodeURIComponent(title)}`;

                return (
                  <div key={item.id} className="my-list-item">
                    <div
                      className="my-list-item-image"
                      onClick={() => handleItemClick(item)}
                    >
                      <img src={imageSrc} alt={title} />
                      <div className={`status-badge status-${item.status}`}>
                        {statusNames[item.status]}
                      </div>
                      <div className="my-list-item-overlay">
                        <h3>{title}</h3>
                        <p className="item-type">
                          {item.titleType === 0 ? 'Movie' : 'Series'}
                        </p>
                      </div>
                    </div>
                    <button
                      className="remove-btn"
                      onClick={() => handleRemove(item.id)}
                    >
                      Remove
                    </button>
                  </div>
                );
              })}
            </div>
          ) : (
            <div className="empty-list">
              <h2>No items in this category</h2>
              <p>Start adding movies and shows to your list!</p>
              <button className="browse-btn" onClick={() => navigate('/movies')}>
                Browse Movies
              </button>
            </div>
          )}
        </div>
      </main>
    </>
  );
};

export default MyList;
