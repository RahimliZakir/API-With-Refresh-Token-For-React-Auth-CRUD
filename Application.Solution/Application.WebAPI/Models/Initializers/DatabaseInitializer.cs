using Application.WebAPI.Models.DataContexts;

namespace Application.WebAPI.Models.Initializers
{
    public static class DatabaseInitializer
    {
        public static IApplicationBuilder Initialize(this IApplicationBuilder app)
        {
            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                VehicleDbContext? db = scope.ServiceProvider.GetService<VehicleDbContext>();

                if (db != null)
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
