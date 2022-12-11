using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Application.Modules.AccountModule;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("signin"), AllowAnonymous]
        [SwaggerOperation("Daxil olma pəncərəsi", "Bu hissədən istifadə edərək, istifadəçi sistemə daxil ola bilər.")]
        async public Task<IActionResult> SignIn(SignInCommand command)
        {
            CommandJsonResponse response = await mediator.Send(command);

            if (response.Error)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("register"), AllowAnonymous]
        [SwaggerOperation("Qeydiyyat pəncərəsi", "Bu hissədən istifadə edərək, istifadəçi sistem üçün qeydiyyatdan keçə bilər.")]
        async public Task<IActionResult> Register(RegisterCommand command)
        {
            CommandJsonResponse response = await mediator.Send(command);

            if (response.Error)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("refresh-token"), AllowAnonymous]
        async public Task<IActionResult> RefreshToken()
        {
            CommandJsonResponse response = await mediator.Send(new RefreshTokenCommand());

            if (response.Error)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
