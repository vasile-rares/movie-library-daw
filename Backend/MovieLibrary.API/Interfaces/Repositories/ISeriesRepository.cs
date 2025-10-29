using MovieLibrary.API.Models;

namespace MovieLibrary.API.Interfaces.Repositories
{
  public interface ISeriesRepository
  {
    Task<IEnumerable<Series>> GetAllAsync();
    Task<Series?> GetByIdAsync(int id);
    Task<IEnumerable<Series>> GetByGenreAsync(int genreId);
    Task<Series> CreateAsync(Series series);
    Task<Series> UpdateAsync(Series series);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task AddGenresToSeriesAsync(int seriesId, List<int> genreIds);
    Task RemoveGenresFromSeriesAsync(int seriesId);
  }
}
