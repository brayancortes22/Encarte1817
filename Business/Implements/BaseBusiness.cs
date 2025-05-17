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
    public class BaseBusiness<TDto, TEntity> : ABaseBusiness<TEntity>, IBaseBusiness<TDto, TEntity>
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


        public override async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation($"Obteniendo todos los registros de {typeof(TEntity).Name}");
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener registros de {typeof(TEntity).Name}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TDto>> GetAllDtoAsync()
        {
            try
            {
                _logger.LogInformation($"Mapeando todos los registros de {typeof(TEntity).Name} a DTOs");
                var entities = await GetAllAsync();
                return _mapper.Map<IEnumerable<TDto>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al mapear registros de {typeof(TEntity).Name} a DTOs: {ex.Message}");
                throw;
            }
        }

        public override async Task<TEntity> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Obteniendo {typeof(TEntity).Name} con ID: {id}");
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener {typeof(TEntity).Name} con ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<TDto> GetDtoByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Mapeando {typeof(TEntity).Name} con ID: {id} a DTO");
                var entity = await GetByIdAsync(id);
                return _mapper.Map<TDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al mapear {typeof(TEntity).Name} con ID {id} a DTO: {ex.Message}");
                throw;
            }
        }

        public override async Task<TEntity> CreateAsync(TEntity entity)
        {
            try
            {
                _logger.LogInformation($"Creando nuevo {typeof(TEntity).Name}");
                return await _repository.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear {typeof(TEntity).Name}: {ex.Message}");
                throw;
            }
        }

        public async Task<TDto> CreateAsync(TDto dto)
        {
            try
            {
                _logger.LogInformation($"Creando nuevo {typeof(TEntity).Name} desde DTO");

                EnsureValid(dto);

                var entity = _mapper.Map<TEntity>(dto);
                var result = await CreateAsync(entity);
                return _mapper.Map<TDto>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear {typeof(TEntity).Name} desde DTO: {ex.Message}");
                throw;
            }
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity)
        {
            try
            {
                _logger.LogInformation($"Actualizando {typeof(TEntity).Name}");
                return await _repository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar {typeof(TEntity).Name}: {ex.Message}");
                throw;
            }
        }

        public async Task<TDto> UpdateAsync(TDto dto)
        {
            try
            {
                _logger.LogInformation($"Actualizando {typeof(TEntity).Name} desde DTO");

                EnsureValid(dto);

                var entity = _mapper.Map<TEntity>(dto);
                var result = await UpdateAsync(entity);
                return _mapper.Map<TDto>(result);
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
