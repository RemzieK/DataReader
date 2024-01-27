using DataReader.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataReader.API.Controllers
{
    [ApiController]
    public abstract class BaseController<T> : ControllerBase where T : class
    {
        protected readonly IRepository<T> _baseRepository;

        public BaseController(IRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<T>>> GetAll()
        {
            var entities = await _baseRepository.GetAllAsync();
            return Ok(entities);
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<ActionResult<T>> GetById(int id)
        {
            var entity = await _baseRepository.GetByIdAsync(id);
            if (entity != null)
            {
                return Ok(entity);
            }
            else
            {
                return NotFound($"{typeof(T).Name} not found");
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> Create(T entity)
        {
            await _baseRepository.CreateAsync(entity);
            return Ok();
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult> Update(T entity)
        {
            await _baseRepository.UpdateAsync(entity);
            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<ActionResult> Delete(int id)
        {
            await _baseRepository.DeleteAsync(id);
            return Ok();
        }
    }
}
