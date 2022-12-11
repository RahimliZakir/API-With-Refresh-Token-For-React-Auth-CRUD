using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Application.Modules.BusModule;
using Application.WebAPI.AppCode.Mappers.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusesController : ControllerBase
    {
        readonly IMediator mediator;

        public BusesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetBuses()
        {
            IEnumerable<BusDto> dto = await mediator.Send(new BusGetAllActiveQuery());

            return Ok(dto);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetBus([FromRoute] BusSingleQuery query)
        {
            CommandJsonResponse response = await mediator.Send(query);

            if (response.Error)
                return BadRequest(response.Message);

            return Ok(((CommandJsonResponse<BusDto>)response).Data);
        }

        [HttpPost]
        public async Task<IActionResult> PostBus([FromForm] BusCreateCommand command)
        {
            CommandJsonResponse response = await mediator.Send(command);

            if (response.Error)
                return BadRequest(response.Message);

            return CreatedAtAction("GetBus", new { Id = ((CommandJsonResponse<BusDto>)response).Data.Id }, response);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutBus([FromForm] BusEditCommand command)
        {
            CommandJsonResponse response = await mediator.Send(command);

            if (response.Error)
                return BadRequest(response.Message);

            return AcceptedAtAction("GetBus", new { Id = ((CommandJsonResponse<BusDto>)response).Data.Id }, response);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteBus([FromRoute] BusRemoveCommand command)
        {
            CommandJsonResponse response = await mediator.Send(command);

            if (response.Error)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }
    }
}
