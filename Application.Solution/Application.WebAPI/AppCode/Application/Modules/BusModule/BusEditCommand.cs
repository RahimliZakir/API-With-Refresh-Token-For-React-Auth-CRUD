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
using Microsoft.EntityFrameworkCore;

namespace Application.WebAPI.AppCode.Application.Modules.BusModule
{
    public class BusEditCommand : BusFormModel, IRequest<CommandJsonResponse>
    {
        public class BusEditCommandHandler : IRequestHandler<BusEditCommand, CommandJsonResponse>
        {
            readonly VehicleDbContext db;
            readonly IHostEnvironment env;
            readonly IActionContextAccessor ctx;
            readonly IMapper mapper;

            public BusEditCommandHandler(VehicleDbContext db, IHostEnvironment env, IActionContextAccessor ctx, IMapper mapper)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
                this.mapper = mapper;
            }

            async public Task<CommandJsonResponse> Handle(BusEditCommand request, CancellationToken cancellationToken)
            {
                if (request.Id is null or < 0)
                    return new CommandJsonResponse("Id düzgün göndərilməyib!", true);

                Bus? entity = await db.Buses.FirstOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

                if (entity is null)
                    return new CommandJsonResponse("Məlumat tapılmadı!", true);

                if (ctx.IsValid() == false)
                    return new CommandJsonResponse("Məlumatlar düzgün göndərilməyib!", true);

                if (ctx.IsValid())
                {
                    Bus bus = mapper.Map(request, entity);

                    string? fullpath = null;
                    string? currentpath = null;

                    if (request.File is null && !string.IsNullOrEmpty(request.FileTemp))
                    {
                        bus.ImagePath = entity.ImagePath;
                    }
                    else if (request.File is null)
                    {
                        currentpath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "buses", entity.ImagePath);
                    }
                    else if (request.File is not null)
                    {
                        string ext = Path.GetExtension(request.File.FileName);
                        string filename = $"bus-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                        fullpath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "buses", filename);

                        using (FileStream fs = new(fullpath, FileMode.Create, FileAccess.Write))
                        {
                            await request.File.CopyToAsync(fs, cancellationToken);
                        }

                        bus.ImagePath = filename;
                    }

                    await db.SaveChangesAsync(cancellationToken);

                    BusDto dto = mapper.Map<BusDto>(bus);

                    return new CommandJsonResponse<BusDto>("Məlumatlar uğurla yeniləndi!", false, dto);
                }

                return new CommandJsonResponse("Xətalı müraciət!", true);
            }
        }
    }
}
