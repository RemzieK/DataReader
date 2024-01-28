using DataReader.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [Route("Create")]
        public async Task<ActionResult> Create(T entity)
        {
            await _baseRepository.CreateAsync(entity);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        [Route("Update")]
        public async Task<ActionResult> Update(T entity)
        {
            await _baseRepository.UpdateAsync(entity);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("SoftDelete")]
        public async Task<ActionResult> SoftDelete(int id)
        {
            await _baseRepository.SoftDeleteAsync(id);
            return Ok();
        }

    }
}
