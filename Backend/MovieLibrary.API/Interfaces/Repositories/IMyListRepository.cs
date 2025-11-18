using MovieLibrary.API.Models;

namespace MovieLibrary.API.Interfaces.Repositories
{
    public interface IMyListRepository
    {
        Task<IEnumerable<MyList>> GetAllAsync();
        Task<MyList?> GetByIdAsync(int id);
        Task<IEnumerable<MyList>> GetByUserIdAsync(int userId);
        Task<MyList?> GetByUserAndTitleAsync(int userId, int titleId);
        Task<MyList> CreateAsync(MyList myList);
        Task<MyList> UpdateAsync(MyList myList);
        Task<bool> DeleteAsync(int id);
    }
}
