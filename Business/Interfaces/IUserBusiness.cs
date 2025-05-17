using Business.Interfaces;
using Entity.Dtos.UserDTO;
using Entity.Model;

public interface IUserBusiness : IBaseBusiness<User, UserDto>
{
    Task<User> GetByEmailAsync(string email);
    Task<bool> ValidateCredentialsAsync(string email, string password);
    Task<bool> UpdateParcialUserAsync(UserUpdateDto dto);
    Task<bool> SetUserActiveAsync(UserStatusDto dto);
}
