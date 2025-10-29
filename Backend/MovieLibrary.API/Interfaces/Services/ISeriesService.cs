using MovieLibrary.API.DTOs.Requests.Series;
using MovieLibrary.API.DTOs.Responses.Series;

namespace MovieLibrary.API.Interfaces.Services
{
  public interface ISeriesService
  {
    Task<IEnumerable<SeriesResponseDto>> GetAllSeriesAsync();
    Task<SeriesResponseDto?> GetSeriesByIdAsync(int id);
    Task<IEnumerable<SeriesResponseDto>> GetSeriesByGenreAsync(int genreId);
    Task<SeriesResponseDto> CreateSeriesAsync(CreateSeriesDto dto);
    Task<SeriesResponseDto> UpdateSeriesAsync(int id, UpdateSeriesDto dto);
    Task<bool> DeleteSeriesAsync(int id);
  }
}


