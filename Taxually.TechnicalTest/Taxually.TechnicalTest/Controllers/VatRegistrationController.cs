using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services;

namespace Taxually.TechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VATRegistrationController : ControllerBase
    {
        private readonly IVATRegisterService _vatRegisterService;

        public VATRegistrationController(IVATRegisterService vatRegisterService)
        {
            _vatRegisterService = vatRegisterService;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] VATRegistrationRequest request)
        {
            try
            {
                await _vatRegisterService.RegisterForVAT(request);
                return Ok();
            }
            catch (ArgumentException argumentException)
            {
                return BadRequest($"Invalid input: {argumentException.Message}");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"VAT Registration failed: {e.Message}");
            }
        }
    }
}