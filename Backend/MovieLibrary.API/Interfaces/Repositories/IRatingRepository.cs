using MovieLibrary.API.Models;

namespace MovieLibrary.API.Interfaces.Repositories
{
  public interface IRatingRepository
  {
    Task<IEnumerable<Rating>> GetAllAsync();
    Task<Rating?> GetByIdAsync(int id);
    Task<IEnumerable<Rating>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Rating>> GetByMovieIdAsync(int movieId);
    Task<IEnumerable<Rating>> GetBySeriesIdAsync(int seriesId);
    Task<Rating> CreateAsync(Rating rating);
    Task<Rating> UpdateAsync(Rating rating);
    Task<bool> DeleteAsync(int id);
  }
}
