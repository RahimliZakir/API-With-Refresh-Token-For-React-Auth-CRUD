using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Extensions;
using Application.WebAPI.AppCode.Mappers.Dtos;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;
using Application.WebAPI.Models.FormModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NuGet.Protocol.Plugins;

namespace Application.WebAPI.AppCode.Application.Modules.CarModule
{
    public class CarCreateCommand : CarFormModel, IRequest<CommandJsonResponse>
    {
        public class CarCreateCommandHandler : IRequestHandler<CarCreateCommand, CommandJsonResponse>
        {
            readonly VehicleDbContext db;
            readonly IHostEnvironment env;
            readonly IActionContextAccessor ctx;
            readonly IMapper mapper;

            public CarCreateCommandHandler(VehicleDbContext db, IHostEnvironment env, IActionContextAccessor ctx, IMapper mapper)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
                this.mapper = mapper;
            }

            async public Task<CommandJsonResponse> Handle(CarCreateCommand request, CancellationToken cancellationToken)
            {
                if (ctx.IsValid() == false)
                    return new CommandJsonResponse("Məlumatlar düzgün göndərilməyib!", true);

                if (ctx.IsValid())
                {
                    Car car = mapper.Map<Car>(request);

                    if (request.Files != null && request.Files.Any())
                    {
                        car.CarImages = new List<CarImage>();

                        foreach (ImageItem item in request.Files)
                        {
                            string extension = Path.GetExtension(item.File.FileName);
                            string filepath = $"car-{Guid.NewGuid().ToString().Replace("-", "")}{extension}".ToLower();
                            string fullpath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "cars", filepath);

                            using (FileStream fs = new(fullpath, FileMode.Create, FileAccess.Write))
                            {
                                await item.File.CopyToAsync(fs, cancellationToken);
                            }

                            car.CarImages.Add(new CarImage
                            {
                                ImagePath = filepath,
                                IsMain = item.IsMain
                            });
                        }
                    }

                    await db.Cars.AddAsync(car, cancellationToken);
                    await db.SaveChangesAsync(cancellationToken);

                    CarDto dto = mapper.Map<CarDto>(car);

                    return new CommandJsonResponse<CarDto>("Məlumatlar uğurla əlavə olundu!", false, dto);
                }

                return new CommandJsonResponse("Xətalı müraciət!", true);
            }
        }
    }
}
