using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Business.Services;
using Entity.Dtos.UserDTO;
using Entity.Model;
using Microsoft.Extensions.Logging;

public class UserBusiness : BaseBusiness<User, UserDto>, IUserBusiness
{
    private readonly IUserData _userData;

    public UserBusiness(IUserData userData, IMapper mapper, ILogger<UserBusiness> logger)
        : base(userData, mapper, logger)
    {
        _userData = userData;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var users = await _userData.GetAllAsync();
        return users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<bool> ValidateCredentialsAsync(string email, string password)
    {
        var user = await GetByEmailAsync(email);
        if (user == null || !user.Active)
            return false;

        string hashedPassword = HashPassword(password);
        return user.Password == hashedPassword;
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public override async Task<UserDto> CreateAsync(UserDto dto)
    {
        var user = _mapper.Map<User>(dto);
        user.Password = HashPassword(user.Password);
        user.Active = true;
        user.CreateAt = DateTime.Now;

        return await base.CreateAsync(dto);
    }
    public async Task<bool> UpdateParcialUserAsync(UserUpdateDto dto)
    {
        if (dto == null || dto.Id <= 0)
            throw new ValidationException("Id", "Datos inválidos para actualizar el usuario");

        var exists = await _userData.GetByIdAsync(dto.Id)
            ?? throw new EntityNotFoundException("user", dto.Id);

        return await _userData.PatchRolAsync(dto.Id, dto.Username, dto.Email);
    }


    public async Task<bool> SetUserActiveAsync(UserStatusDto dto)
    {
        if (dto == null || dto.Id <= 0)
            throw new ValidationException("Id", "El ID del usuario es inválido");

        var exists = await _userData.GetByIdAsync(dto.Id)
            ?? throw new EntityNotFoundException("user", dto.Id);

        return await _userData.SetActiveAsync(dto.Id, dto.Active);
    }

}
