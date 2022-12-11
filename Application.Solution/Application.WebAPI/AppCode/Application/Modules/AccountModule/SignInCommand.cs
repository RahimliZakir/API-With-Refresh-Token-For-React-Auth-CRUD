﻿using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Extensions;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities.Membership;
using Application.WebAPI.Models.FormModels;
using Application.WebAPI.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.WebAPI.AppCode.Application.Modules.AccountModule
{
    public class SignInCommand : SignInFormModel, IRequest<CommandJsonResponse>
    {
        public class SignInCommandHandler : IRequestHandler<SignInCommand, CommandJsonResponse>
        {
            readonly VehicleDbContext db;
            readonly UserManager<VehicleUser> userManager;
            readonly SignInManager<VehicleUser> signInManager;
            readonly IConfiguration conf;
            readonly IHttpContextAccessor ctx;

            public SignInCommandHandler(VehicleDbContext db, UserManager<VehicleUser> userManager, SignInManager<VehicleUser> signInManager, IConfiguration conf, IHttpContextAccessor ctx)
            {
                this.db = db;
                this.userManager = userManager;
                this.signInManager = signInManager;
                this.conf = conf;
                this.ctx = ctx;
            }

            async public Task<CommandJsonResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    return new CommandJsonResponse("İstifadəçi adı göndərilməyib!", true);
                }
                else if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return new CommandJsonResponse("Şifrə göndərilməyib!", true);
                }

                VehicleUser? user = null;
                if (request.Username.IsEmail())
                {
                    user = await userManager.FindByEmailAsync(request.Username);
                }
                else
                {
                    user = await userManager.FindByNameAsync(request.Username);
                }

                if (user == null)
                {
                    return new CommandJsonResponse("İstifadəçi adı və ya şifrə xətalıdır!", true);
                }

                SignInResult result = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);

                if (result.IsLockedOut)
                {
                    return new CommandJsonResponse("Hesabınız keçici olaraq məhdudlaşıdırılıb!", true);
                }
                else if (result.IsNotAllowed)
                {
                    return new CommandJsonResponse("Hesabınız məhdudlaşıdırılıb!", true);
                }
                else if (result.Succeeded)
                    goto stopGenerate;
                else
                {
                    return new CommandJsonResponse("Giriş hüququnuz yoxdur!", true);
                }

            stopGenerate:
                string tokenAndExpires = user.GenerateToken(conf);

                // Refresh Token
                RefreshTokenViewModel refreshToken = Extension.GenerateRefreshToken();

                await user.SetRefreshToken(refreshToken, ctx, db, cancellationToken);
                // Refresh Token

                return new CommandJsonResponse<string>("Uğurludur!", false, tokenAndExpires);
            }
        }
    }
}
