using Business.Interfaces;
using Entity.Dtos.RolDTO;
using Entity.Model;

public interface IRolBusiness : IBaseBusiness<Rol, RolDto>
{
    Task<bool> UpdatePartialRolAsync(RolUpdateDto dto);
    Task<bool> DeleteLogicRolAsync(int id);
}
