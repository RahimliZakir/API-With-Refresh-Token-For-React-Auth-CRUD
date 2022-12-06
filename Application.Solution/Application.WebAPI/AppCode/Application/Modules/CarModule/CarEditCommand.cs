using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Extensions;
using Application.WebAPI.AppCode.Mappers.Dtos;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;
using Application.WebAPI.Models.FormModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.WebAPI.AppCode.Application.Modules.CarModule
{
    public class CarEditCommand : CarFormModel, IRequest<CommandJsonResponse>
    {
        public class CarEditCommandHandler : IRequestHandler<CarEditCommand, CommandJsonResponse>
        {
            readonly VehicleDbContext db;
            readonly IHostEnvironment env;
            readonly IActionContextAccessor ctx;
            readonly IMapper mapper;

            public CarEditCommandHandler(VehicleDbContext db, IHostEnvironment env, IActionContextAccessor ctx, IMapper mapper)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
                this.mapper = mapper;
            }

            async public Task<CommandJsonResponse> Handle(CarEditCommand request, CancellationToken cancellationToken)
            {
                if (request.Id is null or < 0)
                    return new CommandJsonResponse("Id düzgün göndərilməyib!", true);

                Car? entity = await db.Cars
                                      .Include(c => c.CarImages)
                                      .FirstOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

                if (entity is null)
                    return new CommandJsonResponse("Məlumat tapılmadı!", true);

                if (ctx.IsValid() == false)
                    return new CommandJsonResponse("Məlumatlar düzgün göndərilməyib!", true);

                if (ctx.IsValid())
                {
                    foreach (ImageItem item in request.Files)
                    {
                        if (item.Id != null && item.Id > 0)
                        {
                            CarImage? current = await db.CarImages.FirstOrDefaultAsync(f => f.Id == item.Id, cancellationToken);

                            if (current != null)
                            {
                                if (string.IsNullOrWhiteSpace(item.TempPath))
                                {
                                    db.CarImages.Remove(current);
                                }
                                else
                                {
                                    current.IsMain = item.IsMain;
                                }
                            }
                        }
                        else if (item.File != null)
                        {
                            string extension = Path.GetExtension(item.File.FileName);
                            string filepath = $"car-{Guid.NewGuid().ToString().Replace("-", "")}{extension}".ToLower();
                            string fullpath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "cars", filepath);

                            using (FileStream fs = new(fullpath, FileMode.Create, FileAccess.Write))
                            {
                                await item.File.CopyToAsync(fs, cancellationToken);
                            }

                            entity.CarImages.Add(new CarImage
                            {
                                ImagePath = filepath,
                                IsMain = item.IsMain
                            });
                        }
                    }

                    Car car = mapper.Map(request, entity);

                    await db.SaveChangesAsync(cancellationToken);

                    CarDto dto = mapper.Map<CarDto>(car);

                    return new CommandJsonResponse<CarDto>("Məlumatlar uğurla yeniləndi!", false, dto);
                }

                return new CommandJsonResponse("Xətalı müraciət!", true);
            }
        }
    }
}
