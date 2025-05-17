using Data.Interfaces;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Business.Implements
{

    /// <summary>
    /// Esta clase genérica sirve como base para servicios de negocio que encapsulan lógica CRUD sobre cualquier entidad,
    /// promoviendo reutilización de código y separación de responsabilidades.
    /// 
    /// Implementa el patrón de diseño Repository y actúa como capa intermedia entre los controladores
    /// y la capa de acceso a datos, añadiendo lógica de negocio y manejo de excepciones.
    /// </summary>
    /// <typeparam name="TEntity">Tipo de entidad sobre la que operará esta clase de negocio. Debe ser una clase.</typeparam>
    /// <remarks>
    /// Las clases que hereden de esta clase base deberán implementar lógica de negocio específica
    /// para cada entidad, mientras que la funcionalidad CRUD básica ya está implementada aquí.
    /// Todos los métodos son virtuales para permitir su sobrescritura en clases derivadas si es necesario.
    /// </remarks>
    public abstract class ABaseBusiness<TEntity> where TEntity : class
    {

        /// <summary>
        /// Repositorio que proporciona las operaciones de acceso a datos para la entidad.
        /// </summary>
        /// <remarks>
        /// Es responsable de ejecutar las operaciones CRUD contra la fuente de datos.
        /// Se inyecta a través del constructor para seguir el principio de inversión de dependencias.
        /// </remarks>
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
        /// <remarks>
        /// Este método implementa manejo de errores registrando cualquier excepción que ocurra
        /// durante la obtención de datos y luego relanzando la excepción para permitir
        /// que capas superiores puedan manejarla adecuadamente.
        /// </remarks>
        /// <exception cref="Exception">
        /// Propaga cualquier excepción que ocurra en la capa de acceso a datos después de registrarla.
        /// </exception>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener todos los registros de {typeof(TEntity).Name}: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Recupera una entidad específica por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único de la entidad a recuperar.</param>
        /// <returns>
        /// Tarea asíncrona que representa la operación y contiene la entidad solicitada
        /// cuando se completa correctamente. Puede retornar null si no se encuentra la entidad.
        /// </returns>
        /// <remarks>
        /// La implementación depende de la implementación subyacente de IBaseData para determinar
        /// el comportamiento cuando no se encuentra la entidad (puede retornar null o lanzar una excepción).
        /// </remarks>
        /// <exception cref="Exception">
        /// Propaga cualquier excepción que ocurra en la capa de acceso a datos después de registrarla.
        /// </exception>
        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener {typeof(TEntity).Name} con ID {id}: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Crea una nueva entidad en la fuente de datos.
        /// </summary>
        /// <param name="entity">Entidad a crear. No debe ser null.</param>
        /// <returns>
        /// Tarea asíncrona que representa la operación y contiene la entidad creada con sus
        /// valores actualizados (como el ID generado) cuando se completa correctamente.
        /// </returns>
        /// <remarks>
        /// El comportamiento exacto depende de la implementación del repositorio subyacente.
        /// Típicamente, se asigna un identificador a la entidad y se devuelve la entidad actualizada.
        /// </remarks>
        /// <exception cref="Exception">
        /// Propaga cualquier excepción que ocurra en la capa de acceso a datos después de registrarla.
        /// </exception>
        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            try
            {
                return await _repository.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear {typeof(TEntity).Name}: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Actualiza una entidad existente en la fuente de datos.
        /// </summary>
        /// <param name="entity">Entidad con los valores actualizados. No debe ser null.</param>
        /// <returns>
        /// Tarea asíncrona que representa la operación y contiene la entidad actualizada
        /// cuando se completa correctamente.
        /// </returns>
        /// <remarks>
        /// El repositorio subyacente debe encargarse de verificar si la entidad existe.
        /// Si la entidad no existe, el comportamiento dependerá de la implementación específica
        /// (podría lanzar una excepción o crear una nueva entidad).
        /// </remarks>
        /// <exception cref="Exception">
        /// Propaga cualquier excepción que ocurra en la capa de acceso a datos después de registrarla.
        /// </exception>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            try
            {
                return await _repository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar {typeof(TEntity).Name}: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Elimina una entidad de la fuente de datos por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único de la entidad a eliminar.</param>
        /// <returns>
        /// Tarea asíncrona que representa la operación y contiene un valor booleano que indica
        /// si la eliminación fue exitosa (true) o si la entidad no existía (false).
        /// </returns>
        /// <remarks>
        /// El comportamiento exacto cuando la entidad no existe depende de la implementación
        /// del repositorio subyacente, pero típicamente retorna false en lugar de lanzar una excepción.
        /// </remarks>
        /// <exception cref="Exception">
        /// Propaga cualquier excepción que ocurra en la capa de acceso a datos después de registrarla.
        /// </exception>
        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
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