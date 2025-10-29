using MovieLibrary.API.Models;

namespace MovieLibrary.API.Interfaces.Repositories
{
  public interface IGenreRepository
  {
    Task<IEnumerable<Genre>> GetAllAsync();
    Task<Genre?> GetByIdAsync(int id);
    Task<Genre?> GetByNameAsync(string name);
    Task<Genre> CreateAsync(Genre genre);
    Task<Genre> UpdateAsync(Genre genre);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
  }
}
