using Business.Interfaces;
using Data.Interfaces;
using Entity;
using Entity.Model;
using Exceptions;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Business.Implements
{
    public class RoleUserBusiness : ABaseBusiness<RolUser>, IRoleUserBusiness
    {
        public RoleUserBusiness(IBaseData<RolUser> repository, ILogger<RoleUserBusiness> logger)
            : base(repository, logger)
        {
        }

        public async Task<bool> UpdateParcialRoleUserAsync(int id, int roleId)
        {
            if (id <= 0 || roleId <= 0)
                throw new ValidationException("Parámetros inválidos para actualizar la asignación de rol");

            var exists = await _repository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("role_user", id);

            exists.RoleId = roleId;
            return await _repository.UpdatePartialAsync(exists);
        }
    }
}
