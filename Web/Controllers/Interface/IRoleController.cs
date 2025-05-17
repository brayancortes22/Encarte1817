using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.RolDTO;
using Entity.Model;

namespace Web.Controllers.Interface
{
    public interface IRoleController : IGenericController<RolDto, Rol>
    {
        Task<IActionResult> UpdatePartialRole(RolUpdateDto dto);
        Task<IActionResult> DeleteLogicRole(int id);
    }
}