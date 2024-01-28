using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataReader.Domain.Interfaces;
using DataReader;
using System.Threading.Tasks;
using DataReader.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace DataReader.API.Controllers
{
    [Route("apiIndustryController")]
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
        [Authorize]
        [Route("CreateIndustry")]
        public async Task<ActionResult<int>> CreateIndustry(Industry industry)
        {
            await _industryRepository.CreateAsync(industry);
            return Ok(industry.IndustryId); 
        }


        [HttpPost]
        [Authorize]
        [Route("UpdateIndustry")]
        public async Task<ActionResult<int>> UpdateIndustry(Industry industry)
        {
            
            await _industryRepository.UpdateAsync(industry);
            return Ok(industry.IndustryId); 
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("SoftDeleteIndustry")]
        public async Task<ActionResult> SoftDeleteIndustry(int industryId)
        {
            try
            {
                await _industryRepository.SoftDeleteAsync(industryId);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound("Industry not found");
            }
        }

    }
}
