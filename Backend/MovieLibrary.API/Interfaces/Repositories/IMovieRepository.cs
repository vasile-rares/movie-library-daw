using MovieLibrary.API.Models;

namespace MovieLibrary.API.Interfaces.Repositories
{
  public interface IMovieRepository
  {
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<Movie?> GetByIdAsync(int id);
    Task<IEnumerable<Movie>> GetByGenreAsync(int genreId);
    Task<Movie> CreateAsync(Movie movie);
    Task<Movie> UpdateAsync(Movie movie);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task AddGenresToMovieAsync(int movieId, List<int> genreIds);
    Task RemoveGenresFromMovieAsync(int movieId);
  }
}
