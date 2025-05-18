using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Microsoft.Extensions.Logging;
using Utilities.Interfaces;
using Data.Interfaces;
using FluentValidation.Results;



namespace Business.Implements
{
    /// <sumary>
    ///
    </sumary>
    public class BaseBusiness<TDto, TEntity> : ABaseBusiness<TDto, TEntity>
        where TEntity : class
    {
        protected readonly IMapper _mapper;
        protected readonly IGenericIHelpers _helpers;

        public BaseBusiness(
            IBaseData<TEntity> repository,
            ILogger logger,
            IMapper mapper,
            IGenericIHelpers helpers)
            : base(repository, logger)
        {
            _mapper = mapper;
            _helpers = helpers;

        }

        protected async Task EnsureValid(TDto dto)
        {
            var validationResult = await _helpers.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors);
                throw new ArgumentException($"Validación fallida: {errors}");
            }
        }

        public override async Task<List<TDto>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                _logger.LogInformation($"Obteniendo todos los registros de {typeof(TEntity).Name}");
                return _mapper.Map<IList<TDto>>(entities).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener registros de {typeof(TEntity).Name}: {ex.Message}");
                throw;
            }
        }

        public override async Task<TDto> GetByIdAsync(int id)
        {
            try
            {
                var entities = await _repository.GetByIdAsync(id);
                _logger.LogInformation($"Obteniendo {typeof(TEntity).Name} con ID: {id}");
                return _mapper.Map<TDto>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener {typeof(TEntity).Name} con ID {id}: {ex.Message}");
                throw;
            }
        }


        public  override async Task<TDto> CreateAsync(TDto dto)
        {
            try
            {
                EnsureValid(dto);

                var entity = _mapper.Map<TEntity>(dto);
                entity = await _repository.CreateAsync(entity);
                _logger.LogInformation($"Creando nuevo {typeof(TEntity).Name}");
                return _mapper.Map<TDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear {typeof(TEntity).Name} desde DTO: {ex.Message}");
                throw;
            }
        }


        public override async Task<TDto> UpdateAsync(TDto dto)
        {
            try
            {
                _logger.LogInformation($"Actualizando {typeof(TEntity).Name} desde DTO");

                EnsureValid(dto);

                var entity = _mapper.Map<TEntity>(dto);
                return _mapper.Map<TDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar {typeof(TEntity).Name} desde DTO: {ex.Message}");
                throw;
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Eliminando {typeof(TEntity).Name} con ID: {id}");
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar {typeof(TEntity).Name} con ID {id}: {ex.Message}");
                throw;
            }
        }
    }
}