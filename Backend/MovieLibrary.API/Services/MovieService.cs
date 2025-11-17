using AutoMapper;
using MovieLibrary.API.DTOs.Requests.Movie;
using MovieLibrary.API.DTOs.Responses.Movie;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Interfaces.Services;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Services
{
  public class MovieService : IMovieService
  {
    private readonly IMovieRepository _movieRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IMapper _mapper;

    public MovieService(IMovieRepository movieRepository, IGenreRepository genreRepository, IMapper mapper)
    {
      _movieRepository = movieRepository;
      _genreRepository = genreRepository;
      _mapper = mapper;
    }

    public async Task<IEnumerable<MovieResponseDto>> GetAllMoviesAsync()
    {
      var movies = await _movieRepository.GetAllAsync();
      return _mapper.Map<IEnumerable<MovieResponseDto>>(movies);
    }

    public async Task<MovieResponseDto?> GetMovieByIdAsync(int id)
    {
      var movie = await _movieRepository.GetByIdAsync(id);
      return movie == null ? null : _mapper.Map<MovieResponseDto>(movie);
    }

    public async Task<IEnumerable<MovieResponseDto>> GetMoviesByGenreAsync(int genreId)
    {
      var movies = await _movieRepository.GetByGenreAsync(genreId);
      return _mapper.Map<IEnumerable<MovieResponseDto>>(movies);
    }

    public async Task<MovieResponseDto> CreateMovieAsync(CreateMovieDto dto)
    {
      var movie = new Movie
      {
        Title = dto.Title,
        Description = dto.Description,
        ReleaseYear = dto.ReleaseYear,
        ImageUrl = dto.ImageUrl
      };

      var createdMovie = await _movieRepository.CreateAsync(movie);

      if (dto.GenreIds.Any())
      {
        await _movieRepository.AddGenresToMovieAsync(createdMovie.Id, dto.GenreIds);
      }

      // Reload to get genres
      var movieWithGenres = await _movieRepository.GetByIdAsync(createdMovie.Id);
      return _mapper.Map<MovieResponseDto>(movieWithGenres!);
    }

    public async Task<MovieResponseDto> UpdateMovieAsync(int id, UpdateMovieDto dto)
    {
      var movie = await _movieRepository.GetByIdAsync(id);
      if (movie == null)
        throw new KeyNotFoundException($"Movie with ID {id} not found");

      if (!string.IsNullOrEmpty(dto.Title))
        movie.Title = dto.Title;

      if (dto.Description != null)
        movie.Description = dto.Description;

      if (dto.ReleaseYear.HasValue)
        movie.ReleaseYear = dto.ReleaseYear;

      if (dto.ImageUrl != null)
        movie.ImageUrl = dto.ImageUrl;

      await _movieRepository.UpdateAsync(movie);

      if (dto.GenreIds != null)
      {
        await _movieRepository.RemoveGenresFromMovieAsync(id);
        if (dto.GenreIds.Any())
        {
          await _movieRepository.AddGenresToMovieAsync(id, dto.GenreIds);
        }
      }

      // Reload to get updated genres
      var updatedMovie = await _movieRepository.GetByIdAsync(id);
      return _mapper.Map<MovieResponseDto>(updatedMovie!);
    }

    public async Task<bool> DeleteMovieAsync(int id)
    {
      return await _movieRepository.DeleteAsync(id);
    }
  }
}
