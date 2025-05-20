using Business.Interfaces;
using Data.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Implements
{

    public abstract class ABaseBusiness<TDto, TEntity> : IBaseBusiness<TDto, TEntity> where TEntity : class
    {

        protected readonly IBaseData<TEntity> _repository;

        /// <summary>
        /// Servicio de logging para registrar informaci?n, advertencias y errores durante la ejecuci?n.
        /// </summary>
        /// <remarks>
        /// Utiliza la interfaz ILogger de Microsoft.Extensions.Logging para permitir
        /// diferentes implementaciones de logging (consola, archivo, base de datos, etc.).
        /// </remarks>
        protected readonly ILogger _logger;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ABaseBusiness{TEntity}"/>.
        /// </summary>
        /// <param name="repository">Repositorio para operaciones de acceso a datos de la entidad espec?fica.</param>
        /// <param name="logger">Servicio de logging para registrar eventos y errores.</param>
        public ABaseBusiness(IBaseData<TEntity> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene una colecci?n de todas las entidades del tipo especificado.
        /// </summary>
        /// <returns>
        /// Tarea asincrona que representa la operaci?n y contiene una colecci?n de entidades 
        /// cuando se completa correctamente.
        /// </returns>
        public abstract Task<List<TDto>> GetAllAsync();

        /// <summary>
        /// Recupera una entidad espec?fica por su identificador ?nico.
        /// </summary>
        /// <param name="id">Identificador ?nico de la entidad a recuperar.</param>
        /// <returns>
        /// Tarea asincrona que representa la operaci?n y contiene la entidad solicitada
        /// cuando se completa correctamente. Puede retornar null si no se encuentra la entidad.
        /// </returns>
        public abstract Task<TDto> GetByIdAsync(int id);

        /// <summary>
        /// Crea una nueva entidad en la fuente de datos.
        /// </summary>
        /// <param name="entity">Entidad a crear. No debe ser null.</param>
        /// <returns>
        /// Tarea asincrona que representa la operaci?n y contiene la entidad creada con sus
        /// valores actualizados (como el ID generado) cuando se completa correctamente.
        /// </returns>
        public abstract Task<TDto> CreateAsync(TDto dto);

        /// <summary>
        /// Actualiza una entidad existente en la fuente de datos.
        /// </summary>
        /// <param name="entity">Entidad con los valores actualizados. No debe ser null.</param>
        /// <returns>
        /// Tarea asincrona que representa la operaci?n y contiene la entidad actualizada
        /// cuando se completa correctamente.
        /// </returns>
        public abstract Task<TDto> UpdateAsync(TDto dto);

        /// <summary>
        /// Elimina una entidad de la fuente de datos por su identificador ?nico.
        /// </summary>
        /// <param name="id">Identificador ?nico de la entidad a eliminar.</param>
        /// <returns>
        /// Tarea asincrona que representa la operaci?n y contiene un valor booleano que indica
        /// si la eliminaci?n fue exitosa (true) o si la entidad no exist?a (false).
        /// </returns>
        public abstract Task<bool> DeleteAsync(int id);
    }
}