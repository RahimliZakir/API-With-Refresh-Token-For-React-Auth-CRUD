using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Application.Modules.BusModule;
using Application.WebAPI.AppCode.Mappers.Dtos;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WebAPI.AppCode.Application.Modules.BusModule
{
    public class BusSingleQuery : IRequest<CommandJsonResponse>
    {
        public int? Id { get; set; }

        public class BusSingleQueryHandler : IRequestHandler<BusSingleQuery, CommandJsonResponse>
        {
            readonly VehicleDbContext db;
            readonly IMapper mapper;

            public BusSingleQueryHandler(VehicleDbContext db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public async Task<CommandJsonResponse> Handle(BusSingleQuery request, CancellationToken cancellationToken)
            {
                if (request.Id == null || request.Id <= 0)
                    return new CommandJsonResponse("Id düzgün göndərilməyib!", true);

                Bus? bus = await db.Buses.FirstOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

                if (bus is null)
                    return new CommandJsonResponse("Məlumat tapılmadı!", true);

                BusDto dto = mapper.Map<BusDto>(bus);

                return new CommandJsonResponse<BusDto>("Uğrulu!", false, dto);
            }
        }
    }
}
