using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;

namespace Application.WebAPI.Models.Initializers
{
    class CarInitializer
    {
        public static void InitializeCars(VehicleDbContext db)
        {
            if (!db.Cars.Any())
            {
                db.Cars.Add(new Car
                {
                    Company = "Hyundai",
                    Model = "Grandeur"
                });

                db.Cars.Add(new Car
                {
                    Company = "Hummer",
                    Model = "H2"
                });

                db.Cars.Add(new Car
                {
                    Company = "Nissan",
                    Model = "Altima"
                });

                db.SaveChanges();
            }
        }
    }
}
