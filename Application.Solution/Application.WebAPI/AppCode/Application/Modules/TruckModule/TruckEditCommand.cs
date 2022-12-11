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

namespace Application.WebAPI.AppCode.Application.Modules.TruckModule
{
    public class TruckEditCommand : TruckFormModel, IRequest<CommandJsonResponse>
    {
        public class TruckEditCommandHandler : IRequestHandler<TruckEditCommand, CommandJsonResponse>
        {
            readonly VehicleDbContext db;
            readonly IHostEnvironment env;
            readonly IActionContextAccessor ctx;
            readonly IMapper mapper;

            public TruckEditCommandHandler(VehicleDbContext db, IHostEnvironment env, IActionContextAccessor ctx, IMapper mapper)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
                this.mapper = mapper;
            }

            async public Task<CommandJsonResponse> Handle(TruckEditCommand request, CancellationToken cancellationToken)
            {
                if (request.Id is null or < 0)
                    return new CommandJsonResponse("Id düzgün göndərilməyib!", true);

                Truck? entity = await db.Trucks.FirstOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

                if (entity is null)
                    return new CommandJsonResponse("Məlumat tapılmadı!", true);

                if (ctx.IsValid() == false)
                    return new CommandJsonResponse("Məlumatlar düzgün göndərilməyib!", true);

                if (ctx.IsValid())
                {
                    Truck truck = mapper.Map(request, entity);

                    string? fullpath = null;
                    string? currentpath = null;

                    if (request.File is null && !string.IsNullOrEmpty(request.FileTemp))
                    {
                        truck.ImagePath = entity.ImagePath;
                    }
                    else if (request.File is null)
                    {
                        currentpath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "trucks", entity.ImagePath);

                        truck.ImagePath = null;
                    }
                    else if (request.File is not null)
                    {
                        string ext = Path.GetExtension(request.File.FileName);
                        string filename = $"truck-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                        fullpath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "trucks", filename);

                        using (FileStream fs = new(fullpath, FileMode.Create, FileAccess.Write))
                        {
                            await request.File.CopyToAsync(fs, cancellationToken);
                        }

                        truck.ImagePath = filename;
                    }

                    await db.SaveChangesAsync(cancellationToken);

                    TruckDto dto = mapper.Map<TruckDto>(truck);

                    return new CommandJsonResponse<TruckDto>("Məlumatlar uğurla yeniləndi!", false, dto);
                }

                return new CommandJsonResponse("Xətalı müraciət!", true);
            }
        }
    }
}
