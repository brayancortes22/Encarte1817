using System.ComponentModel.DataAnnotations;
using DataAccess.Interfaces; 
using AutoMapper; 
using Microsoft.Extensions.Logging; 
using Entity; 
using Dtos;
using Exceptions;
using Business.Services;
using Entity.Model;
using Entity.Dtos.RolDTO;
using Business.Interfaces;
using Data.Interfaces;


namespace Business.Implements
{
    /// <summary>
    /// Contiene la logica de negocio de los metodos especificos para la entidad Rol
    /// Extiende BaseBusiness heredando la logica de negocio de los metodos base 
    /// </summary>
    public class RolBusiness : BaseBusiness<Rol, RolDto>, IRolBusiness
    {
        ///<summary>Proporciona acceso a los metodos de la capa de datos de roles</summary>
        private readonly IRolData _rolData;

        /// <summary>
        /// Constructor de la clase RolBusiness
        /// Inicializa una nueva instancia con las dependencias necesarias para operar con roles.
        /// </summary>
        public RolBusiness(IRolData rolData, IMapper mapper, ILogger<RolBusiness> logger)
            : base(rolData, mapper, logger)
        {
            _rolData = rolData;
        }

        ///<summary>
        /// Actualiza parcialmente un rol en la base de datos
        /// </summary>
        public async Task<bool> UpdatePartialRolAsync(UpdateRolDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ValidationException("Id", "Datos inválidos");

            var exists = await _rolData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException("rol", dto.Id);

            return await _rolData.PatchRolAsync(dto.Id, dto.Name);
        }

        ///<summary>
        /// Desactiva un rol en la base de datos
        /// </summary>
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