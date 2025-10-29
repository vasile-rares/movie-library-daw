using MovieLibrary.API.DTOs.Requests.Auth;
using MovieLibrary.API.DTOs.Responses.Auth;

namespace MovieLibrary.API.Interfaces.Services;

public interface IAuthService
{
  Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
  Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
}
