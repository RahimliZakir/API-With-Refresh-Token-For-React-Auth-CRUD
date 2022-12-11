using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Application.WebAPI.Models.Initializers
{
    public static class DatabaseInitializer
    {
        async public static Task<IApplicationBuilder> InitializeMembership(this IApplicationBuilder app)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
            {
                RoleManager<VehicleRole>? roleManager = serviceScope.ServiceProvider.GetService<RoleManager<VehicleRole>>();
                UserManager<VehicleUser>? userManager = serviceScope.ServiceProvider.GetService<UserManager<VehicleUser>>();

                VehicleRole? roleResult = await roleManager.FindByNameAsync("Admin");

                if (roleResult is null)
                {
                    roleResult = new()
                    {
                        Name = "Admin"
                    };

                    IdentityResult? roleStatus = await roleManager.CreateAsync(roleResult);

                    if (roleStatus.Succeeded)
                    {
                        VehicleUser? userResult = await userManager.FindByNameAsync("zakir");

                        if (userResult is null)
                        {
                            userResult = new()
                            {
                                UserName = "zakir"
                            };

                            IdentityResult? userStatus = await userManager.CreateAsync(userResult, "zakir007");

                            if (userStatus.Succeeded)
                            {
                                await userManager.AddToRoleAsync(userResult, roleResult.Name);
                            }
                        }
                        else
                        {
                            await userManager.AddToRoleAsync(userResult, roleResult.Name);
                        }
                    }
                }
                else
                {
                    VehicleUser? userResult = await userManager.FindByNameAsync("zakir");

                    if (userResult is null)
                    {
                        userResult = new()
                        {
                            UserName = "zakir",
                            Email = "zakirer@code.edu.az"
                        };

                        IdentityResult? userStatus = await userManager.CreateAsync(userResult, "zakir007");

                        if (userStatus.Succeeded)
                        {
                            await userManager.AddToRoleAsync(userResult, roleResult.Name);
                        }
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(userResult, roleResult.Name);
                    }
                }
            }

            return app;

        }


        public static IApplicationBuilder Initialize(this IApplicationBuilder app)
        {
            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                VehicleDbContext? db = scope.ServiceProvider.GetService<VehicleDbContext>();

                if (db is not null)
                {
                    TruckInitializer.InitializeTrucks(db);
                    BusInitializer.InitializeBuses(db);
                    CarInitializer.InitializeCars(db);
                    CarImageInitializer.InitializeCarImages(db);
                }
            }

            return app;
        }
    }
}
