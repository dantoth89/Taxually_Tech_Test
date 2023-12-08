using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Models.Enum;

namespace Taxually.TechnicalTest.Services;

public class VATRegisterService : IVATRegisterService
{
    private readonly TaxuallyHttpClient _httpClient;
    private readonly TaxuallyQueueClient _queueClient;
    private readonly IFormatService _formatService;
    private const string QueueName = "vat-registration-";

    public VATRegisterService(
        TaxuallyHttpClient httpClient,
        TaxuallyQueueClient queueClient,
        IFormatService formatService)
    {
        _httpClient = httpClient;
        _queueClient = queueClient;
        _formatService = formatService;
    }


    public async Task RegisterForVAT(VATRegistrationRequest request)
    {
        var countryCode = ParseCountryCode(request.Country);

        switch (countryCode)
        {
            case CountryCode.GB:
                await RegisterForVATInUK(request);
                break;
            case CountryCode.FR:
                await RegisterForVATInFrance(request);
                break;
            case CountryCode.DE:
                await RegisterForVATInGermany(request);
                break;
            default:
                throw new Exception("Country not supported");
        }
    }

    private async Task RegisterForVATInUK(VATRegistrationRequest request)
    {
        await _httpClient.PostAsync("https://api.uktax.gov.uk", request);
    }

    private async Task RegisterForVATInFrance(VATRegistrationRequest request)
    {
        var csv = _formatService.GenerateCsv(request);
        await _queueClient.EnqueueAsync(QueueName + FileType.csv, csv);
    }

    private async Task RegisterForVATInGermany(VATRegistrationRequest request)
    {
        var xml = _formatService.GenerateXml(request);
        await _queueClient.EnqueueAsync(QueueName + FileType.xml, xml);
    }

    private static CountryCode ParseCountryCode(string countryCode)
    {
        if (Enum.TryParse<CountryCode>(countryCode, out var result))
        {
            return result;
        }

        throw new ArgumentException("Invalid country code", nameof(countryCode));
    }
}