using MovieLibrary.API.DTOs.Requests.MyList;
using MovieLibrary.API.DTOs.Responses.MyList;

namespace MovieLibrary.API.Interfaces.Services
{
  public interface IMyListService
  {
    Task<IEnumerable<MyListResponseDto>> GetAllAsync();
    Task<MyListResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<MyListResponseDto>> GetByUserIdAsync(int userId);
    Task<MyListResponseDto> AddToMyListAsync(AddToMyListDto dto);
    Task<MyListResponseDto> UpdateStatusAsync(int id, UpdateMyListStatusDto dto);
    Task<bool> RemoveFromMyListAsync(int id);
  }
}
