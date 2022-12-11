using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Application.Modules.BusModule;
using Application.WebAPI.AppCode.Extensions;
using Application.WebAPI.AppCode.Mappers.Dtos;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;
using Application.WebAPI.Models.FormModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Application.WebAPI.AppCode.Application.Modules.BusModule
{
    public class BusCreateCommand : BusFormModel, IRequest<CommandJsonResponse>
    {
        public class BusCreateCommandHandler : IRequestHandler<BusCreateCommand, CommandJsonResponse>
        {
            readonly VehicleDbContext db;
            readonly IHostEnvironment env;
            readonly IActionContextAccessor ctx;
            readonly IMapper mapper;

            public BusCreateCommandHandler(VehicleDbContext db, IHostEnvironment env, IActionContextAccessor ctx, IMapper mapper)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
                this.mapper = mapper;
            }

            async public Task<CommandJsonResponse> Handle(BusCreateCommand request, CancellationToken cancellationToken)
            {
                if (ctx.IsValid() != true)
                    return new CommandJsonResponse("Məlumatlar düzgün göndərilməyib!", true);

                if (ctx.IsValid())
                {
                    Bus bus = mapper.Map<Bus>(request);

                    if (request.File != null)
                    {
                        string ext = Path.GetExtension(request.File.FileName);
                        string filename = $"bus-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                        string fullpath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "buses", filename);

                        using (FileStream fs = new(fullpath, FileMode.Open, FileAccess.Write))
                        {
                            await request.File.CopyToAsync(fs, cancellationToken);
                        }

                        bus.ImagePath = filename;
                    }

                    await db.Buses.AddAsync(bus, cancellationToken);
                    await db.SaveChangesAsync(cancellationToken);

                    BusDto dto = mapper.Map<BusDto>(bus);

                    return new CommandJsonResponse<BusDto>("Məlumatlar uğurla əlavə olundu!", false, dto);
                }

                return new CommandJsonResponse("Xətalı müraciət!", true);
            }
        }
    }
}
