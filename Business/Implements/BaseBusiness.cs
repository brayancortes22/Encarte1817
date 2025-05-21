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
    /// <summary>
    /// Clase base que implementa la lógica de negocio común para operaciones CRUD genéricas.
    /// Proporciona implementaciones estándar para crear, leer, actualizar y eliminar entidades,
    /// incluyendo validación, mapeo automático entre DTOs y entidades, y logging.
    /// </summary>
    /// <typeparam name="TDto">Tipo del objeto de transferencia de datos (DTO) utilizado para comunicación con capas superiores</typeparam>
    /// <typeparam name="TEntity">Tipo de la entidad de dominio que representa el modelo de datos</typeparam>
    /// <remarks>
    /// Esta clase hereda de ABaseBusiness y extiende su funcionalidad añadiendo:
    /// - Mapeo automático entre DTOs y entidades usando AutoMapper
    /// - Validación de DTOs usando FluentValidation
    /// - Logging detallado de todas las operaciones
    /// - Manejo consistente de errores
    /// </remarks>
    public class BaseBusiness<TDto, TEntity> : ABaseBusiness<TDto, TEntity>
        where TEntity : class
    {

        /// <summary>
        /// Instancia de AutoMapper para realizar el mapeo entre DTOs y entidades.
        /// </summary>
        protected readonly IMapper _mapper;

        /// <summary>
        /// Servicio de utilidades genéricas que incluye funcionalidades como validación.
        /// </summary>
        protected readonly IGenericIHelpers _helpers;


        /// <summary>
        /// Inicializa una nueva instancia de la clase BaseBusiness.
        /// </summary>
        /// <param name="repository">Repositorio de datos para operaciones de persistencia de la entidad</param>
        /// <param name="logger">Logger para registrar eventos y errores durante las operaciones</param>
        /// <param name="mapper">Instancia de AutoMapper para mapeo entre DTOs y entidades</param>
        /// <param name="helpers">Servicio de utilidades que proporciona funcionalidades como validación</param>
        public BaseBusiness(
            IBaseData<TEntity> repository,
            ILogger logger,
            IMapper mapper,
            IGenericIHelpers helpers)
            : base(repository, logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _helpers = helpers ?? throw new ArgumentNullException(nameof(helpers));
        }


        /// <summary>
        /// Valida un DTO utilizando las reglas de validación de FluentValidation.
        /// </summary>
        /// <param name="dto">El objeto DTO a validar</param>
        /// <returns>Una tarea que representa la operación de validación asíncrona</returns>
        /// <remarks>
        /// Este método utiliza el servicio _helpers para realizar la validación.
        /// Si la validación falla, se agrupan todos los errores en una sola excepción.
        /// </remarks>
        protected async Task EnsureValid(TDto dto)
        {
            var validationResult = await _helpers.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors);
                throw new ArgumentException($"Validación fallida: {errors}");
            }
        }

        /// <summary>
        /// Obtiene todos los registros de la entidad desde el repositorio.
        /// </summary>
        /// <returns>
        /// Una lista de DTOs que representan todas las entidades almacenadas
        /// </returns>
        /// <exception cref="Exception">
        /// Se relanza cualquier excepción que ocurra durante la operación de consulta
        /// </exception>
        /// <remarks>
        /// Este método:
        /// 1. Consulta todos los registros del repositorio
        /// 2. Los mapea automáticamente a DTOs
        /// 3. Registra la operación en el log
        /// 4. Maneja y registra cualquier error que pueda ocurrir
        /// </remarks>
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

        /// <summary>
        /// Obtiene una entidad específica por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único de la entidad a buscar</param>
        /// <returns>
        /// El DTO correspondiente a la entidad encontrada, o null si no existe
        /// </returns>
        /// <exception cref="Exception">
        /// Se relanza cualquier excepción que ocurra durante la operación de consulta
        /// </exception>
        /// <remarks>
        /// Este método busca una entidad específica por ID y la convierte al DTO correspondiente.
        /// Si la entidad no existe, retorna null.
        /// </remarks>
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

        /// <summary>
        /// Crea una nueva entidad en el sistema a partir de un DTO.
        /// </summary>
        /// <param name="dto">El DTO que contiene los datos para crear la nueva entidad</param>
        /// <returns>
        /// El DTO de la entidad creada, incluyendo el ID asignado y cualquier otro campo generado
        /// </returns>
        /// <exception cref="Exception">
        /// Se relanza cualquier excepción que ocurra durante la operación de creación
        /// </exception>
        /// <remarks>
        /// Este método:
        /// 1. Valida el DTO de entrada
        /// 2. Lo mapea a una entidad
        /// 3. Crea la entidad en el repositorio
        /// 4. Mapea la entidad creada de vuelta a DTO y la retorna
        /// 5. Registra la operación y maneja errores
        /// </remarks>
        public override async Task<TDto> CreateAsync(TDto dto)
        {
            try
            {
                await EnsureValid(dto);
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

        /// <summary>
        /// Actualiza una entidad existente con los datos proporcionados en el DTO.
        /// </summary>
        /// <param name="dto">El DTO que contiene los datos actualizados para la entidad</param>
        /// <returns>
        /// El DTO de la entidad actualizada
        /// </returns>
        /// <exception cref="Exception">
        /// Se relanza cualquier excepción que ocurra durante la operación de actualización
        /// </exception>
        /// <remarks>
        /// NOTA: Esta implementación actual solo valida y mapea el DTO, pero no persiste los cambios.
        /// Es probable que falte la llamada a _repository.UpdateAsync(entity) para completar la operación.
        /// </remarks>
        public override async Task<TDto> UpdateAsync(TDto dto)
        {
            try
            {
                
                await EnsureValid(dto);
                var entity = _mapper.Map<TEntity>(dto);
                entity = await _repository.UpdateAsync(entity);
                _logger.LogInformation($"Actualizando {typeof(TEntity).Name} desde DTO");
                return _mapper.Map<TDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar {typeof(TEntity).Name} desde DTO: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Elimina permanentemente una entidad del sistema por su identificador.
        /// </summary>
        /// <param name="id">El identificador único de la entidad a eliminar</param>
        /// <returns>
        /// true si la entidad fue eliminada exitosamente; false en caso contrario
        /// </returns>
        /// <exception cref="Exception">
        /// Se relanza cualquier excepción que ocurra durante la operación de eliminación
        /// </exception>
        /// <remarks>
        /// Esta operación es irreversible y elimina permanentemente la entidad de la base de datos.
        /// Se recomienda verificar la existencia de la entidad antes de intentar eliminarla.
        /// </remarks>
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
