using System.Text;
using System.Xml.Serialization;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Services;

public class FormatService : IFormatService
{
    public byte[] GenerateCsv(VATRegistrationRequest request)
    {
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("CompanyName,CompanyId");
        csvBuilder.AppendLine($"{request.CompanyName},{request.CompanyId}");
        return Encoding.UTF8.GetBytes(csvBuilder.ToString());
    }

    public string GenerateXml(VATRegistrationRequest request)
    {
        using (var stringWriter = new StringWriter())
        {
            var serializer = new XmlSerializer(typeof(VATRegistrationRequest));
            serializer.Serialize(stringWriter, request);
            return stringWriter.ToString();
        }
    }
}