using MovieLibrary.API.DTOs.Requests.Title;
using MovieLibrary.API.DTOs.Responses.Title;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Interfaces.Services
{
  public interface ITitleService
  {
    Task<IEnumerable<TitleResponseDto>> GetAllAsync();
    Task<TitleResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<TitleResponseDto>> GetByTypeAsync(TitleType type);
    Task<IEnumerable<TitleResponseDto>> GetByGenreAsync(int genreId);
    Task<TitleResponseDto> CreateAsync(CreateTitleDto dto);
    Task<TitleResponseDto> UpdateAsync(int id, UpdateTitleDto dto);
    Task<bool> DeleteAsync(int id);
  }
}
