using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;

namespace Application.WebAPI.Models.Initializers
{
    class CarImageInitializer
    {
        public static void InitializeCarImages(VehicleDbContext db)
        {
            if (!db.CarImages.Any())
            {
                db.CarImages.Add(new CarImage
                {
                    ImagePath = "grandeur-1.jpg",
                    IsMain = true,
                    CarId = 1
                });

                db.CarImages.Add(new CarImage
                {
                    ImagePath = "grandeur-2.jpg",
                    IsMain = false,
                    CarId = 1
                });

                db.CarImages.Add(new CarImage
                {
                    ImagePath = "grandeur-3.jpg",
                    IsMain = false,
                    CarId = 1
                });

                db.CarImages.Add(new CarImage
                {
                    ImagePath = "hummer-1.jpg",
                    IsMain = false,
                    CarId = 2
                });

                db.CarImages.Add(new CarImage
                {
                    ImagePath = "hummer-2.jpg",
                    IsMain = true,
                    CarId = 2
                });

                db.CarImages.Add(new CarImage
                {
                    ImagePath = "nissan.jpg",
                    IsMain = true,
                    CarId = 3
                });

                db.SaveChanges();
            }
        }
    }
}
