using Business.Implements;
using Entity;
using Entity.Dtos.RolUserDTO;
using Entity.Model;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRoleUserBusiness : IBaseBusiness<RolUser, RolUserDto>
    {
        Task<bool> UpdateParcialRoleUserAsync(int id, int roleId);
    }
}
