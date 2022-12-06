using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;
using MediatR;
using Application.WebAPI.AppCode.Application.Modules.CarModule;
using Application.WebAPI.AppCode.Mappers.Dtos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Application.WebAPI.AppCode.Application.Infrastructure;
using Azure;

namespace Application.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        readonly IMediator mediator;

        public CarsController(VehicleDbContext context, IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCars()
        {
            IEnumerable<CarDto> dto = await mediator.Send(new CarGetAllActiveQuery());

            return Ok(dto);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCar([FromRoute] CarSingleQuery query)
        {
            CommandJsonResponse response = await mediator.Send(query);

            if (response.Error)
                return BadRequest(response.Message);

            return Ok(((CommandJsonResponse<CarDto>)response).Data);
        }

        [HttpPost]
        public async Task<IActionResult> PostCar([FromForm] CarCreateCommand command)
        {
            CommandJsonResponse response = await mediator.Send(command);

            if (response.Error)
                return BadRequest(response.Message);

            return CreatedAtAction("GetCar", new { Id = ((CommandJsonResponse<CarDto>)response).Data.Id }, response);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutCar([FromForm] CarEditCommand command)
        {
            CommandJsonResponse response = await mediator.Send(command);

            if (response.Error)
                return BadRequest(response.Message);

            return AcceptedAtAction("GetCar", new { Id = ((CommandJsonResponse<CarDto>)response).Data.Id }, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(CarRemoveCommand command)
        {
            CommandJsonResponse response = await mediator.Send(command);

            if (response.Error)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }
    }
}
