using AutoMapper;
using MovieLibrary.API.DTOs.Requests.Title;
using MovieLibrary.API.DTOs.Responses.Title;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Interfaces.Services;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Services
{
  public class TitleService : ITitleService
  {
    private readonly ITitleRepository _titleRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IMapper _mapper;

    public TitleService(ITitleRepository titleRepository, IGenreRepository genreRepository, IMapper mapper)
    {
      _titleRepository = titleRepository;
      _genreRepository = genreRepository;
      _mapper = mapper;
    }

    public async Task<IEnumerable<TitleResponseDto>> GetAllAsync()
    {
      var titles = await _titleRepository.GetAllAsync();
      return _mapper.Map<IEnumerable<TitleResponseDto>>(titles);
    }

    public async Task<TitleResponseDto?> GetByIdAsync(int id)
    {
      var title = await _titleRepository.GetByIdAsync(id);
      return title == null ? null : _mapper.Map<TitleResponseDto>(title);
    }

    public async Task<IEnumerable<TitleResponseDto>> GetByTypeAsync(TitleType type)
    {
      var titles = await _titleRepository.GetByTypeAsync(type);
      return _mapper.Map<IEnumerable<TitleResponseDto>>(titles);
    }

    public async Task<IEnumerable<TitleResponseDto>> GetByGenreAsync(int genreId)
    {
      var titles = await _titleRepository.GetByGenreAsync(genreId);
      return _mapper.Map<IEnumerable<TitleResponseDto>>(titles);
    }

    public async Task<TitleResponseDto> CreateAsync(CreateTitleDto dto)
    {
      // Validate that all genre IDs exist
      if (dto.GenreIds.Any())
      {
        foreach (var genreId in dto.GenreIds)
        {
          var genreExists = await _genreRepository.ExistsAsync(genreId);
          if (!genreExists)
            throw new KeyNotFoundException($"Genre with ID {genreId} not found");
        }
      }

      var title = new Title
      {
        Name = dto.Title,
        Description = dto.Description,
        ReleaseYear = dto.ReleaseYear,
        ImageUrl = dto.ImageUrl,
        Type = dto.Type,
        SeasonsCount = dto.SeasonsCount,
        EpisodesCount = dto.EpisodesCount
      };

      var createdTitle = await _titleRepository.CreateAsync(title);

      if (dto.GenreIds.Any())
      {
        await _titleRepository.AddGenresToTitleAsync(createdTitle.Id, dto.GenreIds);
      }

      // Reload to get genres
      var titleWithGenres = await _titleRepository.GetByIdAsync(createdTitle.Id);
      return _mapper.Map<TitleResponseDto>(titleWithGenres!);
    }

    public async Task<TitleResponseDto> UpdateAsync(int id, UpdateTitleDto dto)
    {
      var title = await _titleRepository.GetByIdAsync(id);
      if (title == null)
        throw new KeyNotFoundException($"Title with ID {id} not found");

      // Validate that all genre IDs exist
      if (dto.GenreIds.Any())
      {
        foreach (var genreId in dto.GenreIds)
        {
          var genreExists = await _genreRepository.ExistsAsync(genreId);
          if (!genreExists)
            throw new KeyNotFoundException($"Genre with ID {genreId} not found");
        }
      }

      title.Name = dto.Title;
      title.Description = dto.Description;
      title.ReleaseYear = dto.ReleaseYear;
      title.ImageUrl = dto.ImageUrl;
      title.Type = dto.Type;
      title.SeasonsCount = dto.SeasonsCount;
      title.EpisodesCount = dto.EpisodesCount;

      await _titleRepository.UpdateAsync(title);

      // Update genres
      await _titleRepository.RemoveGenresFromTitleAsync(id);
      if (dto.GenreIds.Any())
      {
        await _titleRepository.AddGenresToTitleAsync(id, dto.GenreIds);
      }

      // Reload to get updated genres
      var updatedTitle = await _titleRepository.GetByIdAsync(id);
      return _mapper.Map<TitleResponseDto>(updatedTitle!);
    }

    public async Task<bool> DeleteAsync(int id)
    {
      return await _titleRepository.DeleteAsync(id);
    }
  }
}
