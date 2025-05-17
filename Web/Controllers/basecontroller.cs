using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;

namespace Web.Controllers.Implements
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<T, TDto> : ControllerBase where T : class where TDto : class
    {
        protected readonly IBaseBusiness<T, TDto> _business;

        public BaseController(IBaseBusiness<T, TDto> business)
        {
            _business = business;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll()
        {
            var result = await _business.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TDto>> GetById(int id)
        {
            var result = await _business.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<TDto>> Create(TDto dto)
        {
            var result = await _business.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = GetEntityId(result) }, result);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(int id, TDto dto)
        {
            var result = await _business.UpdateAsync(id, dto);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<TDto>> Delete(int id)
        {
            var result = await _business.DeleteAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // Este método debe ser sobrescrito en controladores específicos para extraer el ID del DTO
        protected virtual int GetEntityId(TDto dto)
        {
            // Por defecto asumimos que hay una propiedad Id
            var property = typeof(TDto).GetProperty("Id");
            if (property != null)
            {
                return (int)property.GetValue(dto);
            }
            return 0;
        }
    }
}
