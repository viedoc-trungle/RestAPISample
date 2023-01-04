using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using TeaBreakApi.Controllers.TeaBreaks.v3;
using TeaBreakApi.Data;
using TeaBreakApi.Domain;

namespace TeaBreakApi.Controllers.TeaBreaks.v0
{
    /// <summary>
    /// First version of teabreak management endpoints
    /// </summary>
    [Route("api/v0.1/teabreakservice")]
    public class TeaBreaksV0Controller : ControllerBase
    {
        private readonly TeabreakRepository _teabreakRepository;
        private readonly ProviderRepository _providerRepository;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="teabreakRepository"></param>
        public TeaBreaksV0Controller(TeabreakRepository teabreakRepository, ProviderRepository providerRepository)
        {
            _teabreakRepository = teabreakRepository;
            _providerRepository = providerRepository;
        }

        /// <summary>
        /// Get all scheduled teabreaks
        /// </summary>
        /// <returns>All scheduled teabreaks</returns>
        /// <response code="200">All scheduled teabreaks are successfully retrieved</response>
        [HttpPost("getall", Name = "AllTeaBreaksV0")]
        [ProducesResponseType(typeof(IEnumerable<TeaBreakResponse>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return Ok(_teabreakRepository.GetAll());
        }

        /// <summary>
        /// Add new tea break
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("addnew", Name = "AddTeaBreakV0")]
        public IActionResult AddNew([FromBody] TeaBreakRequest request)
        {
            var teabreak = new TeaBreak()
            {
                Name = request.Name,
                Description = request.Description,
                NumberOfParticipants = request.NumberOfParticipants,
                StartTime = request.StartTime,
                EndTime = request.EndTime
            };

            teabreak = _teabreakRepository.Add(teabreak);

            return Ok(teabreak);
        }

        /// <summary>
        /// Add number of participants
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("addparticipants", Name = "AddParticipantsV0")]
        public IActionResult AddParticipants([FromBody] TeaBreakUpdateRequest request)
        {
            var teaBreak = _teabreakRepository.Get(request.Id);
            if (teaBreak is null)
                return BadRequest("teabreak not found");

            teaBreak.NumberOfParticipants = request.NumberOfParticipants;

            teaBreak = _teabreakRepository.Update(teaBreak);

            return Ok(teaBreak);
        }

        /// <summary>
        /// Add foods and drinks order for the teabreak
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("addorder", Name = "AddOrderV0")]
        public IActionResult AddOrder([FromBody] OrderRequest request)
        {
            var teaBreak = _teabreakRepository.Get(request.TeaBreak);
            if (teaBreak is null)
                return BadRequest("teabreak not found");

            var provider = _providerRepository.Get(request.Provider);
            if (provider is null)
                return BadRequest("provider not found");
            var product = provider.Products.SingleOrDefault(p => p.Id.Equals(request.Product));
            if (product is null)
                return BadRequest("product not found");

            var total = product.Price * request.Quantity;

            teaBreak.Orders.Add(new Order() { Id = Guid.NewGuid(), Product = request.Product, Provider = request.Provider, Quantity = request.Quantity, Total = total });
            return Ok(_teabreakRepository.Update(teaBreak));
        }
    }
}