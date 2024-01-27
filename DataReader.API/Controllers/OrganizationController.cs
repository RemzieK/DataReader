using DataReader.Domain.Entities;
using DataReader.Domain.Interfaces;
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
        [Route("CreateOrganization")]
        public async Task<ActionResult> CreateOrganization(Organization organization)
        {
            await _organizationRepository.CreateAsync(organization);
            return Ok(organization.OrganizationId);
        }

        [HttpPut]
        [Route("UpdateOrganization")]
        public async Task<ActionResult> UpdateOrganization(Organization organization)
        {
            await _organizationRepository.UpdateAsync(organization);
            return Ok(organization.OrganizationId);
        }

        [HttpDelete]
        [Route("DeleteOrganization")]
        public async Task<ActionResult> DeleteOrganization(int id)
        {
            await _organizationRepository.DeleteAsync(id);
            return Ok();
        }
    }
}
