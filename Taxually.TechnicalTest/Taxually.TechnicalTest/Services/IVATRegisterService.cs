using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Services;

public interface IVATRegisterService
{
    Task RegisterForVAT(VATRegistrationRequest request);
}