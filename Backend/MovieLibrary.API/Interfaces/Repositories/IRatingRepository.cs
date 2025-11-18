using MovieLibrary.API.Models;

namespace MovieLibrary.API.Interfaces.Repositories
{
  public interface IRatingRepository
  {
    Task<IEnumerable<Rating>> GetAllAsync();
    Task<Rating?> GetByIdAsync(int id);
    Task<IEnumerable<Rating>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Rating>> GetByTitleIdAsync(int titleId);
    Task<Rating> CreateAsync(Rating rating);
    Task<Rating> UpdateAsync(Rating rating);
    Task<bool> DeleteAsync(int id);
  }
}
