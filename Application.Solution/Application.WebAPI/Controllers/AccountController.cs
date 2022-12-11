﻿using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Application.Modules.AccountModule;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation("Daxil olma pəncərəsi", "Bu hissədən istifadə edərək, istifadəçi sistemə daxil ola bilər.")]
        async public Task<IActionResult> SignIn(SignInCommand command)
        {
            CommandJsonResponse response = await mediator.Send(command);

            if (response.Error)
                return BadRequest(response);

            return Ok(response);
        }
    }
}