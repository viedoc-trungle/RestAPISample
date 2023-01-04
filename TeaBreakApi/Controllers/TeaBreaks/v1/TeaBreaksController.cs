using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TeaBreakApi.Data;
using TeaBreakApi.Domain;

namespace TeaBreakApi.Controllers.TeaBreaks.v3
{
    [Route("api/v1/teabreaks")]
    public class TeaBreaksV1Controller : ControllerBase
    {
        private readonly TeabreakRepository _teabreakRepository;
        private readonly ProviderRepository _providerRepository;

        public TeaBreaksV1Controller(TeabreakRepository teabreakRepository, ProviderRepository providerRepository)
        {
            _teabreakRepository = teabreakRepository;
            _providerRepository = providerRepository;
        }

        /// <summary>
        /// Get all applications
        /// </summary>
        /// <returns>All applications ever applied to our company</returns>
        /// <response code="200">All applications ever applied to our company</response>
        [HttpGet(Name = "AllTeaBreaksV1")]
        [ProducesResponseType(typeof(IEnumerable<TeaBreak>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return Ok(_teabreakRepository.GetAll());
        }

        /// <summary>
        /// Get a teabreak information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetTeaBreakV1")]
        [ProducesResponseType(typeof(TeaBreak), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(Guid id)
        {
            var teaBreak = _teabreakRepository.Get(id);
            if (teaBreak is null)
                return NotFound("teabreak not found");

            return this.HATEOASResult(teaBreak, v => Ok(v));
        }

        /// <summary>
        /// Add a new teabreak
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(Name = "AddTeaBreakV1")]
        [ProducesResponseType(typeof(TeaBreak), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] TeaBreakRequest request)
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
        /// Replace a teabreak
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}", Name = "ReplaceTeaBreakV1")]
        [ProducesResponseType(typeof(TeaBreak), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(Guid id, [FromBody] TeaBreakRequest request)
        {
            var teaBreak = _teabreakRepository.Get(id);
            if (teaBreak is null)
                return NotFound("teabreak not found");

            teaBreak.Name = request.Name;
            teaBreak.Description = request.Description;
            teaBreak.NumberOfParticipants = request.NumberOfParticipants;
            teaBreak.StartTime = request.StartTime;
            teaBreak.EndTime = request.EndTime;

            return Ok(_teabreakRepository.Update(teaBreak));
        }

        /// <summary>
        /// Edit a teabreak
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = "EditTeaBreakV1")]
        [ProducesResponseType(typeof(TeaBreak), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Patch([FromRoute] Guid id, [FromBody] JsonPatchDocument<TeaBreak> patchDocument)
        {
            var teaBreak = _teabreakRepository.Get(id);
            if (teaBreak is null)
                return NotFound("teabreak not found");

            patchDocument.ApplyTo(teaBreak, ModelState);
            return Ok(_teabreakRepository.Update(teaBreak));
        }

        /// <summary>
        /// Add foods and drinks order for the teabreak
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{id}/orders", Name = "AddOrderV1")]
        [ProducesResponseType(typeof(TeaBreak), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddOrder([FromRoute] Guid id, [FromBody] OrderRequest request)
        {
            var teaBreak = _teabreakRepository.Get(id);
            if (teaBreak is null)
                return NotFound("teabreak not found");

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