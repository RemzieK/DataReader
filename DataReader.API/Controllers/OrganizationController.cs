using DataReader.Domain.Entities;
using DataReader.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataReader.API.Controllers
{
    [Route("apiOrganizationController")]
    [ApiController]
    public class OrganizationController : BaseController<Organization>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationController(IOrganizationRepository organizationRepository, IRepository<Organization> baseRepository) : base(baseRepository)
        {
            _organizationRepository = organizationRepository;
        }

        [HttpPost]
        [Authorize]
        [Route("CreateOrganization")]
        public async Task<ActionResult> CreateOrganization(Organization organization)
        {
            await _organizationRepository.CreateAsync(organization);
            return Ok(organization.OrganizationId);
        }

        [HttpPut]
        [Authorize]
        [Route("UpdateOrganization")]
        public async Task<ActionResult> UpdateOrganization(Organization organization)
        {
            await _organizationRepository.UpdateAsync(organization);
            return Ok(organization.OrganizationId);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("SoftDeleteOrganization")]
        public async Task<ActionResult> SoftDeleteOrganization(int id)
        {
            try
            {
                await _organizationRepository.SoftDeleteAsync(id);
                return Ok();
            }
            catch (Exception)
            {
                return NotFound("Ogranization not found");
            }
        }

    }
}
