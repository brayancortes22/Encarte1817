using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.UserDTO;
using Entity.Model;
using Web.Controllers.Interface;

namespace Web.Controllers.Interface
{
    public interface IUserController : IGenericController<UserDto, User>
    {
        Task<IActionResult> GetUserByEmail(string email);
        Task<IActionResult> UpdatePartialUser(UserUpdateDto dto);
        Task<IActionResult> SetUserActive(UserStatusDto dto);
        Task<IActionResult> ValidateCredentials([FromBody] LoginRequestDto loginDto);
    }
}