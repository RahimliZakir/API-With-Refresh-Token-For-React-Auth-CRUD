using Application.WebAPI.AppCode.Extensions;
using Application.WebAPI.AppCode.Providers;
using Application.WebAPI.AppCode.Services.JWT;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities.Membership;
using Application.WebAPI.Models.Initializers;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IConfiguration conf = builder.Configuration;

IServiceCollection services = builder.Services;
services.AddRouting(cfg =>
{
    cfg.LowercaseUrls = true;
});

services.AddControllers(cfg =>
{
    AuthorizationPolicy builder = new AuthorizationPolicyBuilder()
                                      .RequireAuthenticatedUser()
                                      .Build();

    cfg.Filters.Add(new AuthorizeFilter(builder));
});

services.AddDbContext<VehicleDbContext>(cfg =>
{
    cfg.UseSqlServer(conf.GetConnectionString("cString"));
});

services.AddAutoMapper(typeof(Program));

services.AddMediatR(typeof(Program));

services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API with Refresh Token",
        Description = "API with Refresh Token for React",
        //TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Zakir Rahimli",
            Email = "zakirer@code.edu.az",
            Url = new Uri("https://code.edu.az"),
        },
        License = new OpenApiLicense
        {
            Name = "Use under LICX",
            Url = new Uri("https://example.com/license"),
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
     {
           new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference
                 {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                 }
             },
             new string[] {}
     }
    });

    c.EnableAnnotations();
});

services.AddIdentity<VehicleUser, VehicleRole>()
        .AddEntityFrameworkStores<VehicleDbContext>();

services.AddScoped<UserManager<VehicleUser>>()
        .AddScoped<RoleManager<VehicleRole>>()
        .AddScoped<SignInManager<VehicleUser>>();

services.AddScoped<IClaimsTransformation, AppClaimProvider>();

services.AddScoped<IJWTService, JWTService>();

services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
    options.User.RequireUniqueEmail = true;
});

services.AddCors(cfg =>
{
    cfg.AddPolicy("_allowAnyOrigins", options =>
    {
        options.AllowAnyHeader()
               .AllowAnyMethod()
               .AllowAnyOrigin();
    });
});

byte[] buffer = Encoding.UTF8.GetBytes(conf.GetValue<string>("JWT:Secret"));

services.AddAuthentication(cfg =>
{
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = conf.GetValue<string>("JWT:Issuer"),
        ValidAudience = conf.GetValue<string>("JWT:Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(buffer),
        //ClockSkew = TimeSpan.Zero
    };
});

services.AddAuthorization(cfg =>
{
    string[] principals = services.GetPrincipals(typeof(Program));

    foreach (string principal in principals)
    {
        cfg.AddPolicy(principal, options =>
        {
            options.RequireAssertion(assertion =>
             {
                 return assertion.User.HasClaim(principal, "1") || assertion.User.IsInRole("Admin");
             });
        });
    }
});

WebApplication app = builder.Build();
IWebHostEnvironment env = builder.Environment;
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.Initialize();
}

app.UseCors("_allowAnyOrigins");

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API with Refresh Token V1");
});

app.UseStaticFiles();

await app.InitializeMembership();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
