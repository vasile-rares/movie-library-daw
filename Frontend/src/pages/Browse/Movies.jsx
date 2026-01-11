import { useState, useEffect, useMemo } from "react";
import { titleService } from "../../services/titleService";
import { genreService } from "../../services/genreService";
import { myListService } from "../../services/myListService";
import Header from "../../components/Header";
import TitleCard from "../../components/TitleCard";
import "./Browse.css";

const Movies = () => {
  const [allMovies, setAllMovies] = useState([]);
  const [genres, setGenres] = useState([]);
  const [selectedGenre, setSelectedGenre] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [myListByTitleId, setMyListByTitleId] = useState({});

  useEffect(() => {
    fetchData();
    fetchMyList();
  }, []);

  const fetchData = async () => {
    try {
      const [genresRes, moviesRes] = await Promise.all([
        genreService.getAll(),
        titleService.getByType(0),
      ]);
      setGenres(genresRes.data || []);
      setAllMovies(moviesRes.data || []);
    } catch (err) {
      setError("Failed to load movies");
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const fetchMyList = async () => {
    try {
      const response = await myListService.getMyList();
      const map = (response.data || []).reduce((acc, item) => {
        acc[item.titleId] = item.id;
        return acc;
      }, {});
      setMyListByTitleId(map);
    } catch (err) {
      console.error("Failed to load My List", err);
    }
  };

  const handleAddedToList = (createdItem) => {
    if (!createdItem) return;
    setMyListByTitleId((prev) => {
      if (prev[createdItem.titleId]) return prev;
      return { ...prev, [createdItem.titleId]: createdItem.id };
    });
  };

  const handleRemovedFromList = (titleId) => {
    setMyListByTitleId((prev) => {
      if (!prev[titleId]) return prev;
      const next = { ...prev };
      delete next[titleId];
      return next;
    });
  };

  const filteredMovies = useMemo(() => {
    if (!selectedGenre) return allMovies;

    return allMovies.filter((movie) =>
      movie.genres?.some((genre) => genre.id === selectedGenre)
    );
  }, [allMovies, selectedGenre]);

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

  return (
    <>
      <Header />
      <main className="browse-page">
        <div className="browse-header">
          <h1 className="browse-title">Movies</h1>
          <div className="genre-filter-dropdown">
            <label htmlFor="genre-select" className="filter-label">
              Genre:
            </label>
            <select
              id="genre-select"
              className="genre-select"
              value={selectedGenre || ""}
              onChange={(e) =>
                setSelectedGenre(
                  e.target.value ? parseInt(e.target.value) : null
                )
              }
            >
              <option value="">All Genres</option>
              {genres.map((genre) => (
                <option key={genre.id} value={genre.id}>
                  {genre.name}
                </option>
              ))}
            </select>
          </div>
        </div>

        {error && <div className="error-message">{error}</div>}

        <div className="browse-content">
          {filteredMovies.length > 0 ? (
            <div className="content-grid">
              {filteredMovies.map((movie) => (
                <TitleCard
                  key={movie.id}
                  item={movie}
                  type="movie"
                  isInList={!!myListByTitleId[movie.id]}
                  listItemId={myListByTitleId[movie.id]}
                  onAddToList={handleAddedToList}
                  onRemoveFromList={handleRemovedFromList}
                />
              ))}
            </div>
          ) : (
            <div className="no-content">No movies found</div>
          )}
        </div>
      </main>
    </>
  );
};

export default Movies;
