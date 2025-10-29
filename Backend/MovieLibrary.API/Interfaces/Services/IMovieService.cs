using MovieLibrary.API.DTOs.Requests.Movie;
using MovieLibrary.API.DTOs.Responses.Movie;

namespace MovieLibrary.API.Interfaces.Services
{
  public interface IMovieService
  {
    Task<IEnumerable<MovieResponseDto>> GetAllMoviesAsync();
    Task<MovieResponseDto?> GetMovieByIdAsync(int id);
    Task<IEnumerable<MovieResponseDto>> GetMoviesByGenreAsync(int genreId);
    Task<MovieResponseDto> CreateMovieAsync(CreateMovieDto dto);
    Task<MovieResponseDto> UpdateMovieAsync(int id, UpdateMovieDto dto);
    Task<bool> DeleteMovieAsync(int id);
  }
}


