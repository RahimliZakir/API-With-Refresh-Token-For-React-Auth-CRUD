using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Application.Modules.TruckModule;
using Application.WebAPI.AppCode.Mappers.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrucksController : ControllerBase
    {
        readonly IMediator mediator;

        public TrucksController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrucks()
        {
            IEnumerable<TruckDto> dto = await mediator.Send(new TruckGetAllActiveQuery());

            return Ok(dto);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetTruck([FromRoute] TruckSingleQuery query)
        {
            CommandJsonResponse response = await mediator.Send(query);

            if (response.Error)
                return BadRequest(response);

            return Ok(((CommandJsonResponse<TruckDto>)response));
        }

        [HttpPost]
        public async Task<IActionResult> PostTruck([FromForm] TruckCreateCommand command)
        {
            CommandJsonResponse response = await mediator.Send(command);

            if (response.Error)
                return BadRequest(response);

            return CreatedAtAction("GetTruck", new { Id = ((CommandJsonResponse<TruckDto>)response).Data.Id }, response);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutTruck([FromForm] TruckEditCommand command)
        {
            CommandJsonResponse response = await mediator.Send(command);

            if (response.Error)
                return BadRequest(response);

            return AcceptedAtAction("GetTruck", new { Id = ((CommandJsonResponse<TruckDto>)response).Data.Id }, response);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteTruck([FromRoute] TruckRemoveCommand command)
        {
            CommandJsonResponse response = await mediator.Send(command);

            if (response.Error)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
