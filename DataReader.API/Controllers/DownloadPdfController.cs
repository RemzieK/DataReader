using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataReader.API.Controllers
{
    public class DownloadPdfController : Controller
    {
    

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Authorize]
        [HttpGet("download/{organizationId}")]
        public IActionResult DownloadCompanyPdf(string organizationId)
        {
            var pdfStream = _pdfService.GenerateCompanyPdf(organizationId);
            return new FileStreamResult(pdfStream, "application/pdf")
            {
                FileDownloadName = $"Organization_{organizationId}.pdf"
            };
        }

    }
}
