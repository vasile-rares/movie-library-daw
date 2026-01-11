using AutoMapper;
using Moq;
using MovieLibrary.API.DTOs.Requests.Title;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Mappings;
using MovieLibrary.API.Models;
using MovieLibrary.API.Services;

namespace MovieLibrary.Tests.Services;

public class TitleServiceTests
{
  private readonly Mock<ITitleRepository> _mockTitleRepository;
  private readonly Mock<IGenreRepository> _mockGenreRepository;
  private readonly IMapper _mapper;
  private readonly TitleService _titleService;

  public TitleServiceTests()
  {
    _mockTitleRepository = new Mock<ITitleRepository>();
    _mockGenreRepository = new Mock<IGenreRepository>();

    var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
    _mapper = config.CreateMapper();

    _titleService = new TitleService(_mockTitleRepository.Object, _mockGenreRepository.Object, _mapper);
  }

  [Fact]
  public async Task GetAllAsync_ShouldReturnAllTitles()
  {
    var titles = new List<Title>
    {
      new Title
      {
        Id = 1,
        Name = "Test Movie 1",
        Description = "Description 1",
        ReleaseYear = 2023,
        Type = TitleType.Movie,
        TitleGenres = new List<TitleGenre>()
      },
      new Title
      {
        Id = 2,
        Name = "Test Series 1",
        Description = "Description 2",
        ReleaseYear = 2024,
        Type = TitleType.Series,
        SeasonsCount = 3,
        EpisodesCount = 30,
        TitleGenres = new List<TitleGenre>()
      }
    };

    _mockTitleRepository.Setup(r => r.GetAllAsync())
      .ReturnsAsync(titles);

    var result = await _titleService.GetAllAsync();

    Assert.NotNull(result);
    Assert.Equal(2, result.Count());
    _mockTitleRepository.Verify(r => r.GetAllAsync(), Times.Once);
  }

  [Fact]
  public async Task GetByIdAsync_WithValidId_ShouldReturnTitle()
  {
    var title = new Title
    {
      Id = 1,
      Name = "Test Movie",
      Description = "Description",
      ReleaseYear = 2023,
      Type = TitleType.Movie,
      TitleGenres = new List<TitleGenre>()
    };

    _mockTitleRepository.Setup(r => r.GetByIdAsync(1))
      .ReturnsAsync(title);

    var result = await _titleService.GetByIdAsync(1);

    Assert.NotNull(result);
    Assert.Equal("Test Movie", result.Title);
    Assert.Equal(2023, result.ReleaseYear);
    Assert.Equal(TitleType.Movie, result.Type);
  }

  [Fact]
  public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
  {
    _mockTitleRepository.Setup(r => r.GetByIdAsync(999))
      .ReturnsAsync((Title?)null);

    var result = await _titleService.GetByIdAsync(999);

    Assert.Null(result);
  }

  [Fact]
  public async Task GetByTypeAsync_ShouldReturnTitlesOfSpecificType()
  {
    var movies = new List<Title>
    {
      new Title
      {
        Id = 1,
        Name = "Movie 1",
        Type = TitleType.Movie,
        TitleGenres = new List<TitleGenre>()
      },
      new Title
      {
        Id = 2,
        Name = "Movie 2",
        Type = TitleType.Movie,
        TitleGenres = new List<TitleGenre>()
      }
    };

    _mockTitleRepository.Setup(r => r.GetByTypeAsync(TitleType.Movie))
      .ReturnsAsync(movies);

    var result = await _titleService.GetByTypeAsync(TitleType.Movie);

    Assert.NotNull(result);
    Assert.Equal(2, result.Count());
    Assert.All(result, t => Assert.Equal(TitleType.Movie, t.Type));
  }

  [Fact]
  public async Task CreateAsync_WithValidData_ShouldCreateTitle()
  {
    var createDto = new CreateTitleDto
    {
      Title = "New Movie",
      Description = "New Description",
      ReleaseYear = 2024,
      ImageUrl = "http://example.com/image.jpg",
      Type = TitleType.Movie,
      GenreIds = new List<int> { 1, 2 }
    };

    var createdTitle = new Title
    {
      Id = 1,
      Name = createDto.Title,
      Description = createDto.Description,
      ReleaseYear = createDto.ReleaseYear,
      ImageUrl = createDto.ImageUrl,
      Type = createDto.Type,
      TitleGenres = new List<TitleGenre>()
    };

    _mockGenreRepository.Setup(r => r.ExistsAsync(It.IsAny<int>()))
      .ReturnsAsync(true);
    _mockTitleRepository.Setup(r => r.CreateAsync(It.IsAny<Title>()))
      .ReturnsAsync(createdTitle);
    _mockTitleRepository.Setup(r => r.GetByIdAsync(1))
      .ReturnsAsync(createdTitle);

    var result = await _titleService.CreateAsync(createDto);

    Assert.NotNull(result);
    Assert.Equal("New Movie", result.Title);
    Assert.Equal(2024, result.ReleaseYear);
    Assert.Equal(TitleType.Movie, result.Type);
    _mockTitleRepository.Verify(r => r.CreateAsync(It.IsAny<Title>()), Times.Once);
    _mockTitleRepository.Verify(r => r.AddGenresToTitleAsync(1, createDto.GenreIds), Times.Once);
  }

  [Fact]
  public async Task UpdateAsync_WithValidData_ShouldUpdateTitle()
  {
    var existingTitle = new Title
    {
      Id = 1,
      Name = "Old Title",
      Description = "Old Description",
      ReleaseYear = 2020,
      Type = TitleType.Movie,
      TitleGenres = new List<TitleGenre>()
    };

    var updateDto = new UpdateTitleDto
    {
      Title = "Updated Title",
      Description = "Updated Description",
      ReleaseYear = 2024,
      Type = TitleType.Movie,
      GenreIds = new List<int>()
    };

    _mockTitleRepository.Setup(r => r.GetByIdAsync(1))
      .ReturnsAsync(existingTitle);

    var result = await _titleService.UpdateAsync(1, updateDto);

    Assert.NotNull(result);
    Assert.Equal("Updated Title", result.Title);
    _mockTitleRepository.Verify(r => r.UpdateAsync(It.IsAny<Title>()), Times.Once);
  }

  [Fact]
  public async Task DeleteAsync_WithValidId_ShouldReturnTrue()
  {
    _mockTitleRepository.Setup(r => r.DeleteAsync(1))
      .ReturnsAsync(true);

    var result = await _titleService.DeleteAsync(1);

    Assert.True(result);
    _mockTitleRepository.Verify(r => r.DeleteAsync(1), Times.Once);
  }
}
