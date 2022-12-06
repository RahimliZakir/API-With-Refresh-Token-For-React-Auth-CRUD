using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Application.Modules.TruckModule;
using Application.WebAPI.AppCode.Mappers.Dtos;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WebAPI.AppCode.Application.Modules.TruckModule
{
    public class TruckSingleQuery : IRequest<CommandJsonResponse>
    {
        public int? Id { get; set; }

        public class TruckSingleQueryHandler : IRequestHandler<TruckSingleQuery, CommandJsonResponse>
        {
            readonly VehicleDbContext db;
            readonly IMapper mapper;

            public TruckSingleQueryHandler(VehicleDbContext db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public async Task<CommandJsonResponse> Handle(TruckSingleQuery request, CancellationToken cancellationToken)
            {
                if (request.Id == null || request.Id <= 0)
                    return new CommandJsonResponse("Id düzgün göndərilməyib!", true);

                Truck? Truck = await db.Trucks.FirstOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

                if (Truck is null)
                    return new CommandJsonResponse("Məlumat tapılmadı!", true);

                TruckDto dto = mapper.Map<TruckDto>(Truck);

                return new CommandJsonResponse<TruckDto>("Uğrulu!", false, dto);
            }
        }
    }
}
