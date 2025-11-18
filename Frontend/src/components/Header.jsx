import { useState, useEffect, useRef } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import './Header.css';
import { titleService } from '../services/titleService';

const Header = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const [searchQuery, setSearchQuery] = useState('');
  const [searchResults, setSearchResults] = useState([]);
  const [searchOpen, setSearchOpen] = useState(false);
  const [isSearching, setIsSearching] = useState(false);
  const dropdownRef = useRef(null);
  const searchRef = useRef(null);

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const handleSearch = async (query) => {
    setSearchQuery(query);

    if (query.trim().length < 2) {
      setSearchResults([]);
      setSearchOpen(false);
      return;
    }

    setIsSearching(true);
    setSearchOpen(true);

    try {
      const response = await titleService.getAll();
      const allTitles = response.data || [];

      const filtered = allTitles.filter(title =>
        title.title.toLowerCase().includes(query.toLowerCase())
      ).slice(0, 8);

      setSearchResults(filtered);
    } catch (error) {
      console.error('Search error:', error);
      setSearchResults([]);
    } finally {
      setIsSearching(false);
    }
  };

  const handleResultClick = (item) => {
    const type = item.type === 0 ? 'movie' : 'show';
    navigate(`/${type}/${item.id}`);
    setSearchQuery('');
    setSearchResults([]);
    setSearchOpen(false);
  };

  const clearSearch = () => {
    setSearchQuery('');
    setSearchResults([]);
    setSearchOpen(false);
  };

  // Close dropdown and search when clicking outside
  useEffect(() => {
    const handleClickOutside = (event) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setDropdownOpen(false);
      }
      if (searchRef.current && !searchRef.current.contains(event.target)) {
        setSearchOpen(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  return (
    <header className="header">
      <div className="header-container">
        <Link to="/" className="logo">MovieLibrary</Link>

        <nav className="main-nav">
          <Link to="/">Home</Link>
          <Link to="/movies">Movies</Link>
          <Link to="/shows">Shows</Link>
          {user?.role === 'Admin' && <Link to="/admin">Admin</Link>}

          <div className="search-container" ref={searchRef}>
            <div className="search-input-wrapper">
              <svg className="search-icon" width="16" height="16" viewBox="0 0 16 16" fill="none">
                <path d="M7 12C9.76142 12 12 9.76142 12 7C12 4.23858 9.76142 2 7 2C4.23858 2 2 4.23858 2 7C2 9.76142 4.23858 12 7 12Z" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/>
                <path d="M14 14L10.5 10.5" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/>
              </svg>
              <input
                type="text"
                className="search-input"
                placeholder="Search titles..."
                value={searchQuery}
                onChange={(e) => handleSearch(e.target.value)}
                onFocus={() => searchQuery.length >= 2 && setSearchOpen(true)}
              />
              {searchQuery && (
                <button className="search-clear" onClick={clearSearch} type="button">
                  <svg width="14" height="14" viewBox="0 0 14 14" fill="currentColor">
                    <path d="M14 1.41L12.59 0L7 5.59L1.41 0L0 1.41L5.59 7L0 12.59L1.41 14L7 8.41L12.59 14L14 12.59L8.41 7L14 1.41Z"/>
                  </svg>
                </button>
              )}
            </div>

            {searchOpen && (
              <div className="search-results">
                {isSearching ? (
                  <div className="search-loading">Searching...</div>
                ) : searchResults.length > 0 ? (
                  <>
                    {searchResults.map((item) => (
                      <div
                        key={item.id}
                        className="search-result-item"
                        onClick={() => handleResultClick(item)}
                      >
                        <div className="result-poster">
                          <img
                            src={item.imageUrl || `https://via.placeholder.com/40x60/2e51a2/ffffff?text=${item.title.charAt(0)}`}
                            alt={item.title}
                          />
                        </div>
                        <div className="result-info">
                          <div className="result-title">{item.title}</div>
                          <div className="result-meta">
                            <span className="result-type">{item.type === 0 ? 'Movie' : 'Show'}</span>
                            <span className="result-year">{item.releaseYear}</span>
                          </div>
                        </div>
                      </div>
                    ))}
                  </>
                ) : (
                  <div className="search-empty">No results found</div>
                )}
              </div>
            )}
          </div>
        </nav>

        <div className="header-actions">
          <Link to="/my-list" className="my-list-icon" title="My List">
            <svg width="20" height="20" viewBox="0 0 20 20" fill="none">
              <path d="M3 6h14M3 10h14M3 14h14" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/>
            </svg>
          </Link>

          <div className="user-dropdown" ref={dropdownRef}>
            <button
              className="user-trigger"
              onClick={() => setDropdownOpen(!dropdownOpen)}
            >
              <div className="user-avatar">
                <img
                  src={user?.profilePictureUrl || `https://api.dicebear.com/7.x/avataaars/svg?seed=${user?.nickname}`}
                  alt={user?.nickname}
                  onError={(e) => {
                    e.target.src = `https://api.dicebear.com/7.x/avataaars/svg?seed=${user?.nickname}`;
                  }}
                />
              </div>
              <span className="user-nickname">{user?.nickname}</span>
              <svg
                className={`arrow ${dropdownOpen ? 'open' : ''}`}
                width="10"
                height="6"
                viewBox="0 0 10 6"
                fill="currentColor"
              >
                <path d="M1 1L5 5L9 1" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
              </svg>
            </button>

            {dropdownOpen && (
              <div className="dropdown-panel">
                <button
                  className="dropdown-option"
                  onClick={() => {
                    setDropdownOpen(false);
                    navigate('/settings');
                  }}
                >
                  Settings
                </button>
                <div className="dropdown-divider"></div>
                <button
                  className="dropdown-option logout-option"
                  onClick={() => {
                    setDropdownOpen(false);
                    handleLogout();
                  }}
                >
                  Logout
                </button>
              </div>
            )}
          </div>
        </div>
      </div>
    </header>
  );
};

export default Header;
