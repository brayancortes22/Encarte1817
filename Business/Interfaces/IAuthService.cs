// Business/Interfaces/IAuthService.cs
using System.Threading.Tasks;
using Entity.Dtos.AuthDTO;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDto> LoginAsync(LoginRequestDto request);
        Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
        Task<bool> LogoutAsync(int userId);
    }
}


