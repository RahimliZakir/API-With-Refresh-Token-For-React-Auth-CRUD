using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Extensions;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities.Membership;
using Application.WebAPI.Models.FormModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Application.WebAPI.AppCode.Application.Modules.AccountModule
{
    public class RegisterCommand : RegisterFormModel, IRequest<CommandJsonResponse>
    {
        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, CommandJsonResponse>
        {
            readonly IActionContextAccessor ctx;
            readonly UserManager<VehicleUser> userManager;

            public RegisterCommandHandler(IActionContextAccessor ctx, UserManager<VehicleUser> userManager)
            {
                this.ctx = ctx;
                this.userManager = userManager;
            }

            async public Task<CommandJsonResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                if (!ctx.IsValid())
                {
                    return new CommandJsonResponse("Məlumatlar düzgün göndərilməyib!", true);
                }

                // Without Validation Attributes
                //if (string.IsNullOrWhiteSpace(request.Username))
                //{
                //    return new CommandJsonResponse("İstifadəçi adı göndərilməyib!", true);
                //}
                //else if (string.IsNullOrWhiteSpace(request.Email))
                //{
                //    return new CommandJsonResponse("Elektron poçt ünvanı göndərilməyib!", true);
                //}
                //else if (string.IsNullOrWhiteSpace(request.Password))
                //{
                //    return new CommandJsonResponse("Şifrə göndərilməyib!", true);
                //}
                //else if (string.IsNullOrWhiteSpace(request.PasswordConfirm))
                //{
                //    return new CommandJsonResponse("Şifrə təkrarı göndərilməyib!", true);
                //}

                //if (!request.Email.IsEmail())
                //{
                //    return new CommandJsonResponse("Elektron poçt düzgün formatda göndərilməyib!", true);
                //}
                //else if (!request.Password.Equals(request.PasswordConfirm))
                //{
                //    return new CommandJsonResponse("Şifrələr bir-birinə bərabər olmalıdır!", true);
                //}
                // Without Validation Attributes

                if (ctx.IsValid())
                {
                    VehicleUser? user = new()
                    {
                        UserName = request.Username,
                        Email = request.Email
                    };

                    IdentityResult? result = await userManager.CreateAsync(user, request.Password);

                    if (result.Succeeded)
                    {
                        return new CommandJsonResponse("Uğurla qeydiyyatdan keçdiniz!", false);
                    }
                    else
                    {
                        return new CommandJsonResponse("Qeydiyyat zamanı xəta baş verdi!", true);
                    }
                }

                return new CommandJsonResponse("Xətalı müraciət!", true);
            }
        }
    }
}
