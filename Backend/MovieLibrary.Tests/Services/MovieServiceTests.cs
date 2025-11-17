using Moq;
using MovieLibrary.API.DTOs.Requests.Movie;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Models;
using MovieLibrary.API.Services;

namespace MovieLibrary.Tests.Services;

public class MovieServiceTests
{
  private readonly Mock<IMovieRepository> _mockMovieRepository;
  private readonly Mock<IGenreRepository> _mockGenreRepository;
  private readonly MovieService _movieService;

  public MovieServiceTests()
  {
    _mockMovieRepository = new Mock<IMovieRepository>();
    _mockGenreRepository = new Mock<IGenreRepository>();
    _movieService = new MovieService(_mockMovieRepository.Object, _mockGenreRepository.Object);
  }

  [Fact]
  public async Task GetAllMoviesAsync_ShouldReturnAllMovies()
  {
    // Arrange
    var movies = new List<Movie>
    {
      new Movie
      {
        Id = 1,
        Title = "Test Movie 1",
        Description = "Description 1",
        ReleaseYear = 2023,
        MovieGenres = new List<MovieGenre>()
      },
      new Movie
      {
        Id = 2,
        Title = "Test Movie 2",
        Description = "Description 2",
        ReleaseYear = 2024,
        MovieGenres = new List<MovieGenre>()
      }
    };

    _mockMovieRepository.Setup(r => r.GetAllAsync())
      .ReturnsAsync(movies);

    // Act
    var result = await _movieService.GetAllMoviesAsync();

    // Assert
    Assert.NotNull(result);
    Assert.Equal(2, result.Count());
    _mockMovieRepository.Verify(r => r.GetAllAsync(), Times.Once);
  }

  [Fact]
  public async Task GetMovieByIdAsync_WithValidId_ShouldReturnMovie()
  {
    // Arrange
    var movie = new Movie
    {
      Id = 1,
      Title = "Test Movie",
      Description = "Description",
      ReleaseYear = 2023,
      MovieGenres = new List<MovieGenre>()
    };

    _mockMovieRepository.Setup(r => r.GetByIdAsync(1))
      .ReturnsAsync(movie);

    // Act
    var result = await _movieService.GetMovieByIdAsync(1);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("Test Movie", result.Title);
    Assert.Equal(2023, result.ReleaseYear);
  }

  [Fact]
  public async Task GetMovieByIdAsync_WithInvalidId_ShouldReturnNull()
  {
    // Arrange
    _mockMovieRepository.Setup(r => r.GetByIdAsync(999))
      .ReturnsAsync((Movie?)null);

    // Act
    var result = await _movieService.GetMovieByIdAsync(999);

    // Assert
    Assert.Null(result);
  }

  [Fact]
  public async Task CreateMovieAsync_WithValidData_ShouldCreateMovie()
  {
    // Arrange
    var createDto = new CreateMovieDto
    {
      Title = "New Movie",
      Description = "New Description",
      ReleaseYear = 2024,
      ImageUrl = "http://example.com/image.jpg",
      GenreIds = new List<int> { 1, 2 }
    };

    var createdMovie = new Movie
    {
      Id = 1,
      Title = createDto.Title,
      Description = createDto.Description,
      ReleaseYear = createDto.ReleaseYear,
      ImageUrl = createDto.ImageUrl,
      MovieGenres = new List<MovieGenre>()
    };

    _mockMovieRepository.Setup(r => r.CreateAsync(It.IsAny<Movie>()))
      .ReturnsAsync(createdMovie);
    _mockMovieRepository.Setup(r => r.GetByIdAsync(1))
      .ReturnsAsync(createdMovie);

    // Act
    var result = await _movieService.CreateMovieAsync(createDto);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("New Movie", result.Title);
    Assert.Equal(2024, result.ReleaseYear);
    _mockMovieRepository.Verify(r => r.CreateAsync(It.IsAny<Movie>()), Times.Once);
    _mockMovieRepository.Verify(r => r.AddGenresToMovieAsync(1, createDto.GenreIds), Times.Once);
  }

  [Fact]
  public async Task UpdateMovieAsync_WithValidData_ShouldUpdateMovie()
  {
    // Arrange
    var existingMovie = new Movie
    {
      Id = 1,
      Title = "Old Title",
      Description = "Old Description",
      ReleaseYear = 2020,
      MovieGenres = new List<MovieGenre>()
    };

    var updateDto = new UpdateMovieDto
    {
      Title = "Updated Title",
      Description = "Updated Description",
      ReleaseYear = 2024
    };

    _mockMovieRepository.Setup(r => r.GetByIdAsync(1))
      .ReturnsAsync(existingMovie);

    // Act
    var result = await _movieService.UpdateMovieAsync(1, updateDto);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("Updated Title", result.Title);
    _mockMovieRepository.Verify(r => r.UpdateAsync(It.IsAny<Movie>()), Times.Once);
  }

  [Fact]
  public async Task DeleteMovieAsync_WithValidId_ShouldReturnTrue()
  {
    // Arrange
    _mockMovieRepository.Setup(r => r.DeleteAsync(1))
      .ReturnsAsync(true);

    // Act
    var result = await _movieService.DeleteMovieAsync(1);

    // Assert
    Assert.True(result);
    _mockMovieRepository.Verify(r => r.DeleteAsync(1), Times.Once);
  }
}
