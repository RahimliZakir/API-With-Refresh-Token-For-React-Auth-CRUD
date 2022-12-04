using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;

namespace Application.WebAPI.Models.Initializers
{
    class TruckInitializer
    {
        public static void InitializeTrucks(VehicleDbContext db)
        {
            if (!db.Trucks.Any())
            {
                db.Trucks.Add(new Truck
                {
                    Company = "MAN",
                    Model = "F2000",
                    ImagePath = "man-f2000.jpg"
                });

                db.Trucks.Add(new Truck
                {
                    Company = "Mercedes-Benz",
                    Model = "Axor",
                    ImagePath = "mercedes-axor.jpg"
                });

                db.Trucks.Add(new Truck
                {
                    Company = "KAMAZ",
                    Model = "4310",
                    ImagePath = "kamaz-4310.jpg"
                });

                db.SaveChanges();
            }
        }
    }
}
