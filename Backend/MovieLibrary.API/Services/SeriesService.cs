using MovieLibrary.API.DTOs.Requests.Series;
using MovieLibrary.API.DTOs.Responses.Series;
using MovieLibrary.API.DTOs.Responses.Genre;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Interfaces.Services;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Services
{
  public class SeriesService : ISeriesService
  {
    private readonly ISeriesRepository _seriesRepository;

    public SeriesService(ISeriesRepository seriesRepository)
    {
      _seriesRepository = seriesRepository;
    }

    public async Task<IEnumerable<SeriesResponseDto>> GetAllSeriesAsync()
    {
      var series = await _seriesRepository.GetAllAsync();
      return series.Select(MapToResponseDto);
    }

    public async Task<SeriesResponseDto?> GetSeriesByIdAsync(int id)
    {
      var series = await _seriesRepository.GetByIdAsync(id);
      return series == null ? null : MapToResponseDto(series);
    }

    public async Task<IEnumerable<SeriesResponseDto>> GetSeriesByGenreAsync(int genreId)
    {
      var series = await _seriesRepository.GetByGenreAsync(genreId);
      return series.Select(MapToResponseDto);
    }

    public async Task<SeriesResponseDto> CreateSeriesAsync(CreateSeriesDto dto)
    {
      var series = new Series
      {
        Title = dto.Title,
        Description = dto.Description,
        ReleaseYear = dto.ReleaseYear,
        SeasonsCount = dto.SeasonsCount,
        EpisodesCount = dto.EpisodesCount,
        ImageUrl = dto.ImageUrl
      };

      var createdSeries = await _seriesRepository.CreateAsync(series);

      if (dto.GenreIds.Any())
      {
        await _seriesRepository.AddGenresToSeriesAsync(createdSeries.Id, dto.GenreIds);
      }

      var seriesWithGenres = await _seriesRepository.GetByIdAsync(createdSeries.Id);
      return MapToResponseDto(seriesWithGenres!);
    }

    public async Task<SeriesResponseDto> UpdateSeriesAsync(int id, UpdateSeriesDto dto)
    {
      var series = await _seriesRepository.GetByIdAsync(id);
      if (series == null)
        throw new KeyNotFoundException($"Series with ID {id} not found");

      if (!string.IsNullOrEmpty(dto.Title))
        series.Title = dto.Title;

      if (dto.Description != null)
        series.Description = dto.Description;

      if (dto.ReleaseYear.HasValue)
        series.ReleaseYear = dto.ReleaseYear;

      if (dto.SeasonsCount.HasValue)
        series.SeasonsCount = dto.SeasonsCount;

      if (dto.EpisodesCount.HasValue)
        series.EpisodesCount = dto.EpisodesCount;

      if (dto.ImageUrl != null)
        series.ImageUrl = dto.ImageUrl;

      await _seriesRepository.UpdateAsync(series);

      if (dto.GenreIds != null)
      {
        await _seriesRepository.RemoveGenresFromSeriesAsync(id);
        if (dto.GenreIds.Any())
        {
          await _seriesRepository.AddGenresToSeriesAsync(id, dto.GenreIds);
        }
      }

      var updatedSeries = await _seriesRepository.GetByIdAsync(id);
      return MapToResponseDto(updatedSeries!);
    }

    public async Task<bool> DeleteSeriesAsync(int id)
    {
      return await _seriesRepository.DeleteAsync(id);
    }

    private static SeriesResponseDto MapToResponseDto(Series series)
    {
      return new SeriesResponseDto
      {
        Id = series.Id,
        Title = series.Title,
        Description = series.Description,
        ReleaseYear = series.ReleaseYear,
        SeasonsCount = series.SeasonsCount,
        EpisodesCount = series.EpisodesCount,
        ImageUrl = series.ImageUrl,
        Genres = series.SeriesGenres.Select(sg => new GenreResponseDto
        {
          Id = sg.Genre.Id,
          Name = sg.Genre.Name
        }).ToList()
      };
    }
  }
}
