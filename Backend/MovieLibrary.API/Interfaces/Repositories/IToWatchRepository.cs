using MovieLibrary.API.Models;

namespace MovieLibrary.API.Interfaces.Repositories
{
  public interface IToWatchRepository
  {
    Task<IEnumerable<ToWatchList>> GetAllAsync();
    Task<ToWatchList?> GetByIdAsync(int id);
    Task<IEnumerable<ToWatchList>> GetByUserIdAsync(int userId);
    Task<ToWatchList> CreateAsync(ToWatchList toWatch);
    Task<bool> DeleteAsync(int id);
  }
}
