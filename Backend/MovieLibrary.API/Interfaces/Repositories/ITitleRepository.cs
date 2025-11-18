using MovieLibrary.API.Models;

namespace MovieLibrary.API.Interfaces.Repositories
{
    public interface ITitleRepository
    {
        Task<IEnumerable<Title>> GetAllAsync();
        Task<Title?> GetByIdAsync(int id);
        Task<IEnumerable<Title>> GetByTypeAsync(TitleType type);
        Task<IEnumerable<Title>> GetByGenreAsync(int genreId);
        Task<Title> CreateAsync(Title title);
        Task<Title> UpdateAsync(Title title);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task AddGenresToTitleAsync(int titleId, List<int> genreIds);
        Task RemoveGenresFromTitleAsync(int titleId);
    }
}
