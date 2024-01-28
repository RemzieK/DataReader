using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataReader.Infrastructure.Repositories;
using DataReader;
using System.Threading.Tasks;
using DataReader.Domain.Entities;
using DataReader.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace DataReader.API.Controllers
{
    [Route("apiCountryController")]
    [ApiController]
    public class CountryController : BaseController<Country>
    {
        private readonly ICountryRepository _countryRepository;

        public CountryController(ICountryRepository countryRepository, IRepository<Country> baseRepository) : base(baseRepository)
        {
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<string>> GetAll()
        {
            var countries = await _countryRepository.GetAllAsync();
            return Ok(countries);
        }

        [HttpPost]
        [Authorize]
        [Route("CreateCountry")]
        public async Task<ActionResult<int>> CreateCountry(Country country)
        {
            await _countryRepository.CreateAsync(country);
            return Ok(country.CountryId); 
        }


        [HttpGet]
        [Route("GetCountryById")]
        public async Task<ActionResult<Country>> GetCountryById(int id)
        {
            var country = await _countryRepository.GetByIdAsync(id);
            if (country != null)
            {
                return Ok(country);
            }
            else
            {
                return NotFound("Country not found");
            }
        }

        [HttpPut]
        [Authorize]
        [Route("UpdateCountry")]
        public async Task<ActionResult> UpdateCountry( Country country)
        {
                await _countryRepository.UpdateAsync(country);
                return Ok(country.CountryId);
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("SoftDeleteCountry")]
        public async Task<ActionResult> SoftDeleteCountry(int countryId)
        {
            try
            {
                await _countryRepository.SoftDeleteAsync(countryId);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound("Country not found");
            }
        }



    }
}
