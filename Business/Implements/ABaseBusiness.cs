using Data.Interfaces;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Business.Implements
{

    /// <summary>
    /// Esta clase gen�rica sirve como base para servicios de negocio que encapsulan l�gica CRUD sobre cualquier entidad,
    /// promoviendo reutilizaci�n de c�digo y separaci�n de responsabilidades.
    /// 
    /// Implementa el patr�n de dise�o Repository y act�a como capa intermedia entre los controladores
    /// y la capa de acceso a datos, a�adiendo l�gica de negocio y manejo de excepciones.
    /// </summary>
    /// <typeparam name="TEntity">Tipo de entidad sobre la que operar� esta clase de negocio. Debe ser una clase.</typeparam>
    /// <remarks>
    /// Las clases que hereden de esta clase base deber�n implementar l�gica de negocio espec�fica
    /// para cada entidad, mientras que la funcionalidad CRUD b�sica ya est� implementada aqu�.
    /// Todos los m�todos son virtuales para permitir su sobrescritura en clases derivadas si es necesario.
    /// </remarks>
    public abstract class ABaseBusiness<TEntity> where TEntity : class
    {

        /// <summary>
        /// Repositorio que proporciona las operaciones de acceso a datos para la entidad.
        /// </summary>
        /// <remarks>
        /// Es responsable de ejecutar las operaciones CRUD contra la fuente de datos.
        /// Se inyecta a trav�s del constructor para seguir el principio de inversi�n de dependencias.
        /// </remarks>
        protected readonly IBaseData<TEntity> _repository;


        /// <summary>
        /// Servicio de logging para registrar informaci�n, advertencias y errores durante la ejecuci�n.
        /// </summary>
        /// <remarks>
        /// Utiliza la interfaz ILogger de Microsoft.Extensions.Logging para permitir
        /// diferentes implementaciones de logging (consola, archivo, base de datos, etc.).
        /// </remarks>
        protected readonly ILogger _logger;


        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ABaseBusiness{TEntity}"/>.
        /// </summary>
        /// <param name="repository">Repositorio para operaciones de acceso a datos de la entidad espec�fica.</param>
        /// <param name="logger">Servicio de logging para registrar eventos y errores.</param>
        public ABaseBusiness(IBaseData<TEntity> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }


        /// <summary>
        /// Obtiene una colecci�n de todas las entidades del tipo especificado.
        /// </summary>
        /// <returns>
        /// Tarea as�ncrona que representa la operaci�n y contiene una colecci�n de entidades 
        /// cuando se completa correctamente.
        /// </returns>
        /// <remarks>
        /// Este m�todo implementa manejo de errores registrando cualquier excepci�n que ocurra
        /// durante la obtenci�n de datos y luego relanzando la excepci�n para permitir
        /// que capas superiores puedan manejarla adecuadamente.
        /// </remarks>
        /// <exception cref="Exception">
        /// Propaga cualquier excepci�n que ocurra en la capa de acceso a datos despu�s de registrarla.
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
        /// Recupera una entidad espec�fica por su identificador �nico.
        /// </summary>
        /// <param name="id">Identificador �nico de la entidad a recuperar.</param>
        /// <returns>
        /// Tarea as�ncrona que representa la operaci�n y contiene la entidad solicitada
        /// cuando se completa correctamente. Puede retornar null si no se encuentra la entidad.
        /// </returns>
        /// <remarks>
        /// La implementaci�n depende de la implementaci�n subyacente de IBaseData para determinar
        /// el comportamiento cuando no se encuentra la entidad (puede retornar null o lanzar una excepci�n).
        /// </remarks>
        /// <exception cref="Exception">
        /// Propaga cualquier excepci�n que ocurra en la capa de acceso a datos despu�s de registrarla.
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
        /// Tarea as�ncrona que representa la operaci�n y contiene la entidad creada con sus
        /// valores actualizados (como el ID generado) cuando se completa correctamente.
        /// </returns>
        /// <remarks>
        /// El comportamiento exacto depende de la implementaci�n del repositorio subyacente.
        /// T�picamente, se asigna un identificador a la entidad y se devuelve la entidad actualizada.
        /// </remarks>
        /// <exception cref="Exception">
        /// Propaga cualquier excepci�n que ocurra en la capa de acceso a datos despu�s de registrarla.
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
        /// Tarea as�ncrona que representa la operaci�n y contiene la entidad actualizada
        /// cuando se completa correctamente.
        /// </returns>
        /// <remarks>
        /// El repositorio subyacente debe encargarse de verificar si la entidad existe.
        /// Si la entidad no existe, el comportamiento depender� de la implementaci�n espec�fica
        /// (podr�a lanzar una excepci�n o crear una nueva entidad).
        /// </remarks>
        /// <exception cref="Exception">
        /// Propaga cualquier excepci�n que ocurra en la capa de acceso a datos despu�s de registrarla.
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
        /// Elimina una entidad de la fuente de datos por su identificador �nico.
        /// </summary>
        /// <param name="id">Identificador �nico de la entidad a eliminar.</param>
        /// <returns>
        /// Tarea as�ncrona que representa la operaci�n y contiene un valor booleano que indica
        /// si la eliminaci�n fue exitosa (true) o si la entidad no exist�a (false).
        /// </returns>
        /// <remarks>
        /// El comportamiento exacto cuando la entidad no existe depende de la implementaci�n
        /// del repositorio subyacente, pero t�picamente retorna false en lugar de lanzar una excepci�n.
        /// </remarks>
        /// <exception cref="Exception">
        /// Propaga cualquier excepci�n que ocurra en la capa de acceso a datos despu�s de registrarla.
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