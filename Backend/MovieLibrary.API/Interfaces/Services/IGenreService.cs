using MovieLibrary.API.DTOs.Requests.Genre;
using MovieLibrary.API.DTOs.Responses.Genre;

namespace MovieLibrary.API.Interfaces.Services
{
  public interface IGenreService
  {
    Task<IEnumerable<GenreResponseDto>> GetAllGenresAsync();
    Task<GenreResponseDto?> GetGenreByIdAsync(int id);
    Task<GenreResponseDto> CreateGenreAsync(CreateGenreDto dto);
    Task<GenreResponseDto> UpdateGenreAsync(int id, CreateGenreDto dto);
    Task<bool> DeleteGenreAsync(int id);
  }
}


