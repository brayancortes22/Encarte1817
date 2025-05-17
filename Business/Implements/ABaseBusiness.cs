using Data.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Implements
{

    public abstract class ABaseBusiness<TEntity> where TEntity : class
    {

        protected readonly IBaseData<TEntity> _repository;

        /// <summary>
        /// Servicio de logging para registrar información, advertencias y errores durante la ejecución.
        /// </summary>
        /// <remarks>
        /// Utiliza la interfaz ILogger de Microsoft.Extensions.Logging para permitir
        /// diferentes implementaciones de logging (consola, archivo, base de datos, etc.).
        /// </remarks>
        protected readonly ILogger _logger;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ABaseBusiness{TEntity}"/>.
        /// </summary>
        /// <param name="repository">Repositorio para operaciones de acceso a datos de la entidad específica.</param>
        /// <param name="logger">Servicio de logging para registrar eventos y errores.</param>
        public ABaseBusiness(IBaseData<TEntity> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene una colección de todas las entidades del tipo especificado.
        /// </summary>
        /// <returns>
        /// Tarea asíncrona que representa la operación y contiene una colección de entidades 
        /// cuando se completa correctamente.
        /// </returns>
        public abstract Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Recupera una entidad específica por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único de la entidad a recuperar.</param>
        /// <returns>
        /// Tarea asíncrona que representa la operación y contiene la entidad solicitada
        /// cuando se completa correctamente. Puede retornar null si no se encuentra la entidad.
        /// </returns>
        public abstract Task<TEntity> GetByIdAsync(int id);

        /// <summary>
        /// Crea una nueva entidad en la fuente de datos.
        /// </summary>
        /// <param name="entity">Entidad a crear. No debe ser null.</param>
        /// <returns>
        /// Tarea asíncrona que representa la operación y contiene la entidad creada con sus
        /// valores actualizados (como el ID generado) cuando se completa correctamente.
        /// </returns>
        public abstract Task<TEntity> CreateAsync(TEntity entity);

        /// <summary>
        /// Actualiza una entidad existente en la fuente de datos.
        /// </summary>
        /// <param name="entity">Entidad con los valores actualizados. No debe ser null.</param>
        /// <returns>
        /// Tarea asíncrona que representa la operación y contiene la entidad actualizada
        /// cuando se completa correctamente.
        /// </returns>
        public abstract Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Elimina una entidad de la fuente de datos por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único de la entidad a eliminar.</param>
        /// <returns>
        /// Tarea asíncrona que representa la operación y contiene un valor booleano que indica
        /// si la eliminación fue exitosa (true) o si la entidad no existía (false).
        /// </returns>
        public abstract Task<bool> DeleteAsync(int id);
    }
}