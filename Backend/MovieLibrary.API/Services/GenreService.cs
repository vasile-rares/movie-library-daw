using MovieLibrary.API.DTOs.Requests.Genre;
using MovieLibrary.API.DTOs.Responses.Genre;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Interfaces.Services;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Services
{
  public class GenreService : IGenreService
  {
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
      _genreRepository = genreRepository;
    }

    public async Task<IEnumerable<GenreResponseDto>> GetAllGenresAsync()
    {
      var genres = await _genreRepository.GetAllAsync();
      return genres.Select(MapToResponseDto);
    }

    public async Task<GenreResponseDto?> GetGenreByIdAsync(int id)
    {
      var genre = await _genreRepository.GetByIdAsync(id);
      return genre == null ? null : MapToResponseDto(genre);
    }

    public async Task<GenreResponseDto> CreateGenreAsync(CreateGenreDto dto)
    {
      var existingGenre = await _genreRepository.GetByNameAsync(dto.Name);
      if (existingGenre != null)
        throw new ArgumentException("Genre with this name already exists");

      var genre = new Genre
      {
        Name = dto.Name
      };

      var createdGenre = await _genreRepository.CreateAsync(genre);
      return MapToResponseDto(createdGenre);
    }

    public async Task<GenreResponseDto> UpdateGenreAsync(int id, CreateGenreDto dto)
    {
      var genre = await _genreRepository.GetByIdAsync(id);
      if (genre == null)
        throw new KeyNotFoundException($"Genre with ID {id} not found");

      genre.Name = dto.Name;

      var updatedGenre = await _genreRepository.UpdateAsync(genre);
      return MapToResponseDto(updatedGenre);
    }

    public async Task<bool> DeleteGenreAsync(int id)
    {
      return await _genreRepository.DeleteAsync(id);
    }

    private static GenreResponseDto MapToResponseDto(Genre genre)
    {
      return new GenreResponseDto
      {
        Id = genre.Id,
        Name = genre.Name
      };
    }
  }
}
