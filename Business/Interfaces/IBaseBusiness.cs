using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IBaseBusiness<TDto, TEntity> where TEntity : class
    {
        /// <summary>
        /// Obtiene todas las entidades desde la base de datos.
        /// </summary>
        /// <returns>Una colecci�n de objetos de tipo <typeparamref name="TEntity"/>.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Obtiene todos los datos en forma de DTO.
        /// </summary>
        /// <returns>Una colecci�n de objetos de tipo <typeparamref name="TDto"/>.</returns>
        Task<IEnumerable<TDto>> GetAllDtoAsync();

        /// <summary>
        /// Obtiene una entidad espec�fica por su ID.
        /// </summary>
        /// <param name="id">Identificador �nico de la entidad.</param>
        /// <returns>Un objeto <typeparamref name="TEntity"/> si se encuentra; de lo contrario, <c>null</c>.</returns>
        Task<TEntity> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene un DTO espec�fico por su ID.
        /// </summary>
        /// <param name="id">Identificador �nico del DTO.</param>
        /// <returns>Un objeto <typeparamref name="TDto"/> si se encuentra; de lo contrario, <c>null</c>.</returns>
        Task<TDto> GetDtoByIdAsync(int id);

        /// <summary>
        /// Crea un nuevo registro a partir de un DTO.
        /// </summary>
        /// <param name="dto">Objeto de transferencia con los datos a guardar.</param>
        /// <returns>El DTO creado con sus valores actualizados.</returns>
        Task<TDto> CreateAsync(TDto dto);

        /// <summary>
        /// Actualiza un registro existente a partir de un DTO.
        /// </summary>
        /// <param name="dto">Objeto de transferencia con los datos actualizados.</param>
        /// <returns>El DTO actualizado o una excepci�n si falla.</returns>
        Task<TDto> UpdateAsync(TDto dto);

        ///<summary>
        /// Elimina permanentemente un registro del sistema.
        ///</summary>
        ///<param name= "id">Identificador del registro a marcar como eliminado</param>
        ///<returns>True si la operaci�n fue exitosa; false en caso contrario </returns>
        Task<bool> DeleteAsync(int id);
    }
}