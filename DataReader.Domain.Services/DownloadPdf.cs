using DataReader.Domain.Entities;
using DataReader.Domain.Services.AbstractionServices;
using DataReader.Infrastructure.DatabaseConnection;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DataReader.Domain.Services
{
    public class DownloadPdf : IDownloadPdf
    {

        public MemoryStream GenerateCompanyPdf(Organization organization)
        {
            var memoryStream = new MemoryStream();
            var document = new Document();
            var writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            document.Add(new Paragraph($"Organization Name: {organization.Name}"));
            document.Add(new Paragraph($"Country: {organization.Country.CountryName}"));
            document.Add(new Paragraph($"Industry: {organization.Industry.IndustryName}"));

            document.Close();
            writer.Close();

            memoryStream.Position = 0;
            return memoryStream;
        }

    }


}
