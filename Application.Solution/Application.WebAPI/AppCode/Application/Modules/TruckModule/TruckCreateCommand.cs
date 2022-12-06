using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Extensions;
using Application.WebAPI.AppCode.Mappers.Dtos;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;
using Application.WebAPI.Models.FormModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Application.WebAPI.AppCode.Application.Modules.TruckModule
{
    public class TruckCreateCommand : TruckFormModel, IRequest<CommandJsonResponse>
    {
        public class TruckCreateCommandHandler : IRequestHandler<TruckCreateCommand, CommandJsonResponse>
        {
            readonly VehicleDbContext db;
            readonly IHostEnvironment env;
            readonly IActionContextAccessor ctx;
            readonly IMapper mapper;

            public TruckCreateCommandHandler(VehicleDbContext db, IHostEnvironment env, IActionContextAccessor ctx, IMapper mapper)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
                this.mapper = mapper;
            }

            async public Task<CommandJsonResponse> Handle(TruckCreateCommand request, CancellationToken cancellationToken)
            {
                if (ctx.IsValid() != true)
                    return new CommandJsonResponse("Məlumatlar düzgün göndərilməyib!", true);

                if (ctx.IsValid())
                {
                    Truck truck = mapper.Map<Truck>(request);

                    if (request.File != null)
                    {
                        string ext = Path.GetExtension(request.File.FileName);
                        string filename = $"truck-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                        string fullpath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "trucks", filename);

                        using (FileStream fs = new(fullpath, FileMode.Open, FileAccess.Write))
                        {
                            await request.File.CopyToAsync(fs, cancellationToken);
                        }

                        truck.ImagePath = filename;
                    }

                    await db.Trucks.AddAsync(truck, cancellationToken);
                    await db.SaveChangesAsync(cancellationToken);

                    TruckDto dto = mapper.Map<TruckDto>(truck);

                    return new CommandJsonResponse<TruckDto>("Məlumatlar uğurla əlavə olundu!", false, dto);
                }

                return new CommandJsonResponse("Xətalı müraciət!", true);
            }
        }
    }
}
