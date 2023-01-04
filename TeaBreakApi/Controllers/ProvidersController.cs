using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeaBreakApi.Data;

namespace TeaBreakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly ProviderRepository _providerRepository;

        public ProvidersController(ProviderRepository providerRepository)
        {
            _providerRepository= providerRepository;
        }

        /// <summary>
        /// Get all foods and drinks providers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_providerRepository.GetAll());
        }
    }
}
