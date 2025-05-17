// Business/Interfaces/IJwtService.cs
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Entity.Dtos.AuthDTO;
using Entity.Model;

namespace Business.Interfaces
{
    public interface IJwtService
    {
        Task<TokenResponseDto> GenerateTokenAsync(User user, IEnumerable<string> roles);
        ClaimsPrincipal ValidateToken(string token);
        Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);
    }
}