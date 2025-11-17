using Microsoft.Extensions.Configuration;
using Moq;
using MovieLibrary.API.DTOs.Requests.Auth;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Models;
using MovieLibrary.API.Services;

namespace MovieLibrary.Tests.Services;

public class AuthServiceTests
{
  private readonly Mock<IUserRepository> _mockUserRepository;
  private readonly Mock<IConfiguration> _mockConfiguration;
  private readonly AuthService _authService;

  public AuthServiceTests()
  {
    _mockUserRepository = new Mock<IUserRepository>();
    _mockConfiguration = new Mock<IConfiguration>();

    // Setup configuration mock
    _mockConfiguration.Setup(c => c["JwtSettings:Issuer"]).Returns("TestIssuer");
    _mockConfiguration.Setup(c => c["JwtSettings:Audience"]).Returns("TestAudience");
    _mockConfiguration.Setup(c => c["JwtSettings:SecretKey"]).Returns("ThisIsAVerySecretKeyForTestingPurposesOnly123456");
    _mockConfiguration.Setup(c => c.GetSection("JwtSettings:ExpirationMinutes").Value).Returns("60");

    var mockSection = new Mock<IConfigurationSection>();
    mockSection.Setup(s => s.Value).Returns("60");
    _mockConfiguration.Setup(c => c.GetSection("JwtSettings:ExpirationMinutes")).Returns(mockSection.Object);

    _authService = new AuthService(_mockUserRepository.Object, _mockConfiguration.Object);
  }

  [Fact]
  public async Task RegisterAsync_WithValidData_ShouldCreateUserAndReturnToken()
  {
    // Arrange
    var registerDto = new RegisterRequestDto
    {
      Username = "testuser",
      Email = "test@example.com",
      Password = "Password123!"
    };

    _mockUserRepository.Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
      .ReturnsAsync((User?)null);
    _mockUserRepository.Setup(r => r.GetByUsernameAsync(It.IsAny<string>()))
      .ReturnsAsync((User?)null);
    _mockUserRepository.Setup(r => r.CreateAsync(It.IsAny<User>()))
      .ReturnsAsync((User user) => { user.Id = 1; return user; });

    // Act
    var result = await _authService.RegisterAsync(registerDto);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("testuser", result.Username);
    Assert.Equal("test@example.com", result.Email);
    Assert.NotNull(result.Token);
    Assert.NotEmpty(result.Token);
    _mockUserRepository.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Once);
  }

  [Fact]
  public async Task RegisterAsync_WithExistingEmail_ShouldThrowArgumentException()
  {
    // Arrange
    var registerDto = new RegisterRequestDto
    {
      Username = "testuser",
      Email = "existing@example.com",
      Password = "Password123!"
    };

    var existingUser = new User
    {
      Id = 1,
      Email = "existing@example.com",
      Username = "existinguser",
      PasswordHash = "hashedpassword"
    };

    _mockUserRepository.Setup(r => r.GetByEmailAsync(registerDto.Email))
      .ReturnsAsync(existingUser);

    // Act & Assert
    await Assert.ThrowsAsync<ArgumentException>(
      async () => await _authService.RegisterAsync(registerDto)
    );

    _mockUserRepository.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Never);
  }

  [Fact]
  public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
  {
    // Arrange
    var loginDto = new LoginRequestDto
    {
      Email = "test@example.com",
      Password = "Password123!"
    };

    var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Password123!");
    var user = new User
    {
      Id = 1,
      Email = "test@example.com",
      Username = "testuser",
      PasswordHash = hashedPassword,
      Role = "User"
    };

    _mockUserRepository.Setup(r => r.GetByEmailAsync(loginDto.Email))
      .ReturnsAsync(user);

    // Act
    var result = await _authService.LoginAsync(loginDto);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(user.Username, result.Username);
    Assert.Equal(user.Email, result.Email);
    Assert.NotNull(result.Token);
    Assert.NotEmpty(result.Token);
  }

  [Fact]
  public async Task LoginAsync_WithInvalidPassword_ShouldThrowUnauthorizedException()
  {
    // Arrange
    var loginDto = new LoginRequestDto
    {
      Email = "test@example.com",
      Password = "WrongPassword"
    };

    var hashedPassword = BCrypt.Net.BCrypt.HashPassword("CorrectPassword");
    var user = new User
    {
      Id = 1,
      Email = "test@example.com",
      Username = "testuser",
      PasswordHash = hashedPassword,
      Role = "User"
    };

    _mockUserRepository.Setup(r => r.GetByEmailAsync(loginDto.Email))
      .ReturnsAsync(user);

    // Act & Assert
    await Assert.ThrowsAsync<UnauthorizedAccessException>(
      async () => await _authService.LoginAsync(loginDto)
    );
  }

  [Fact]
  public async Task LoginAsync_WithNonExistentEmail_ShouldThrowUnauthorizedException()
  {
    // Arrange
    var loginDto = new LoginRequestDto
    {
      Email = "nonexistent@example.com",
      Password = "Password123!"
    };

    _mockUserRepository.Setup(r => r.GetByEmailAsync(loginDto.Email))
      .ReturnsAsync((User?)null);

    // Act & Assert
    await Assert.ThrowsAsync<UnauthorizedAccessException>(
      async () => await _authService.LoginAsync(loginDto)
    );
  }
}
