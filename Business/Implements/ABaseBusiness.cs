using Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace Business.Implements
{
    public abstract class ABaseBusiness<TEntity> where TEntity : class
    {
        protected readonly IBaseData<TEntity> _repository;
        protected readonly ILogger _logger;

        public ABaseBusiness(IBaseData<TEntity> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

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