using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Services;

public interface IFormatService
{
    byte[] GenerateCsv(VATRegistrationRequest request);
    string GenerateXml(VATRegistrationRequest request);
}