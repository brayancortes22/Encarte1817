using System.ComponentModel.DataAnnotations;
using DataAccess.Interfaces; 
using AutoMapper; 
using Microsoft.Extensions.Logging; 
using Entity; 
using Dtos;
using Exceptions;


namespace Business.Implements
{


public class RolBusiness : BaseBusiness<Rol, RolDto>, IRolBusiness
{
    private readonly IRolData _rolData;

    public RolBusiness(IRolData rolData, IMapper mapper, ILogger<RolBusiness> logger)
        : base(rolData, mapper, logger)
    {
        _rolData = rolData;
    }

    public async Task<bool> UpdatePartialRolAsync(RolUpdateDto dto)
    {
        if (dto == null || dto.Id <= 0)
            throw new ValidationException("Id", "Datos inválidos");

        var exists = await _rolData.GetByIdAsync(dto.Id)
            ?? throw new EntityNotFoundException("rol", dto.Id);

        return await _rolData.PatchRolAsync(dto.Id, dto.Name);
    }


    public async Task<bool> DeleteLogicRolAsync(int id)
    {
        if (id <= 0)
            throw new ValidationException("Id", "ID inválido");

        var exists = await _rolData.GetByIdAsync(id)
            ?? throw new EntityNotFoundException("rol", id);

        return await _rolData.DeleteLogicAsync(id);
    }

}
}