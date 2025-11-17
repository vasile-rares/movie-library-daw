using AutoMapper;
using MovieLibrary.API.DTOs.Requests.Series;
using MovieLibrary.API.DTOs.Responses.Series;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Interfaces.Services;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Services
{
  public class SeriesService : ISeriesService
  {
    private readonly ISeriesRepository _seriesRepository;
    private readonly IMapper _mapper;

    public SeriesService(ISeriesRepository seriesRepository, IMapper mapper)
    {
      _seriesRepository = seriesRepository;
      _mapper = mapper;
    }

    public async Task<IEnumerable<SeriesResponseDto>> GetAllSeriesAsync()
    {
      var series = await _seriesRepository.GetAllAsync();
      return _mapper.Map<IEnumerable<SeriesResponseDto>>(series);
    }

    public async Task<SeriesResponseDto?> GetSeriesByIdAsync(int id)
    {
      var series = await _seriesRepository.GetByIdAsync(id);
      return series == null ? null : _mapper.Map<SeriesResponseDto>(series);
    }

    public async Task<IEnumerable<SeriesResponseDto>> GetSeriesByGenreAsync(int genreId)
    {
      var series = await _seriesRepository.GetByGenreAsync(genreId);
      return _mapper.Map<IEnumerable<SeriesResponseDto>>(series);
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
      return _mapper.Map<SeriesResponseDto>(seriesWithGenres!);
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
      return _mapper.Map<SeriesResponseDto>(updatedSeries!);
    }

    public async Task<bool> DeleteSeriesAsync(int id)
    {
      return await _seriesRepository.DeleteAsync(id);
    }
  }
}
