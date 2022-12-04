using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;

namespace Application.WebAPI.Models.Initializers
{
    class BusInitializer
    {
        public static void InitializeBuses(VehicleDbContext db)
        {
            if (!db.Buses.Any())
            {
                db.Buses.Add(new Bus
                {
                    Company = "Ford",
                    Model = "Transit",
                    ImagePath = "ford-transit.jpg"
                });

                db.Buses.Add(new Bus
                {
                    Company = "Hyundai",
                    Model = "Universe",
                    ImagePath = "hyundai-universe.jpg"
                });

                db.Buses.Add(new Bus
                {
                    Company = "Nissan",
                    Model = "Civilian",
                    ImagePath = "nissan-civilian.jfif"
                });

                db.SaveChanges();
            }
        }
    }
}
