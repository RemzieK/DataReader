using DataReader.Domain.Interfaces;
using DataReader.Domain.Services.AbstractionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataReader.API.Controllers
{
    [ApiController]
    [Route("api/pdf")]
    public class DownloadPdfController : ControllerBase
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IDownloadPdf _pdfService;

        public DownloadPdfController(IOrganizationRepository organizationRepository, IDownloadPdf pdfService)
        {
            _organizationRepository = organizationRepository;
            _pdfService = pdfService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DownloadCompanyPdf(string organizationId)
        {
            var organizations = await _organizationRepository.GetAllAsync();
            var organization = organizations.FirstOrDefault(o => o.OrganizationId == organizationId);

            if (organization == null)
            {
                return NotFound($"Organization with ID {organizationId} not found");
            }

            var pdfStream = _pdfService.GenerateCompanyPdf(organization);
            return new FileStreamResult(pdfStream, "application/pdf")
            {
                FileDownloadName = $"Organization_{organizationId}.pdf"
            };
        }
    }

}
