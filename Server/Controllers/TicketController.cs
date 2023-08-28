using Microsoft.AspNetCore.Mvc;
using Shared.Tickets;
using Swashbuckle.AspNetCore.Annotations;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [SwaggerOperation("Returns a list of tickets.")]
        [HttpGet]
        public async Task<TicketResult.Index> GetIndex([FromQuery] TicketRequest.Index request)
        {
            return await _ticketService.GetIndexAsync(request);
        }

        [SwaggerOperation("Returns a specific ticket.")]
        [HttpGet("{id}")]
        public async Task<TicketResult.Detail> GetDetail([FromRoute] TicketRequest.Detail request)
        {
            return await _ticketService.GetDetailAsync(request);
        }

        [SwaggerOperation("Creates a new ticket.")]
        [HttpPost]
        public async Task<TicketResult.Create> Create([FromBody] TicketRequest.Create request)
        {
            return await _ticketService.CreateAsync(request);
        }

        [SwaggerOperation("Edites an existing ticket.")]
        [HttpPut("{id}")]
        public async Task<TicketResult.Edit> Edit([FromBody] TicketRequest.Edit request)
        {
            return await _ticketService.EditAsync(request);
        }

        [SwaggerOperation("Deletes an existing ticket.")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] TicketRequest.Delete request)
        {
            await _ticketService.DeleteAsync(request);
            return NoContent();
        }
    }
}
