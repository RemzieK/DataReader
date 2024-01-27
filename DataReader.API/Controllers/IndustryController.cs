using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataReader.Domain.Interfaces;
using DataReader;
using System.Threading.Tasks;
using DataReader.Domain.Entities;

namespace DataReader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndustryController : BaseController<Industry>
    {
        private readonly IIndustryRepository _industryRepository;

        public IndustryController(IIndustryRepository industryRepository, IRepository<Industry> baseRepository) : base(baseRepository)
        {
            _industryRepository = industryRepository;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<string>> GetAll()
        {
            var industries = await _industryRepository.GetAllAsync();
            return Ok(industries);
        }

        [HttpPost]
        [Route("CreateIndustry")]
        public async Task<ActionResult<int>> CreateIndustry(Industry industry)
        {
            await _industryRepository.CreateAsync(industry);
            return Ok(industry.IndustryId); 
        }


        [HttpPost]
        [Route("UpdateIndustry")]
        public async Task<ActionResult<int>> UpdateIndustry(Industry industry)
        {
            
            await _industryRepository.UpdateAsync(industry);
            return Ok(industry.IndustryId); 
        }

        [HttpDelete]
        [Route("DeleteIndustry")]
        public async Task<ActionResult> DeleteIndustry(int industryId)
        {
            try
            {
                await _industryRepository.DeleteAsync(industryId);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound("Industry not found");
            }
        }
    }
}
