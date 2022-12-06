using Application.WebAPI.AppCode.Application.Modules.TruckModule;
using Application.WebAPI.AppCode.Mappers.Dtos;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WebAPI.AppCode.Application.Modules.TruckModule
{
    public class TruckGetAllActiveQuery : IRequest<IEnumerable<TruckDto>>
    {
        public class TruckGetAllActiveQueryHandler : IRequestHandler<TruckGetAllActiveQuery, IEnumerable<TruckDto>>
        {
            readonly VehicleDbContext db;
            readonly IMapper mapper;

            public TruckGetAllActiveQueryHandler(VehicleDbContext db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            async public Task<IEnumerable<TruckDto>> Handle(TruckGetAllActiveQuery request, CancellationToken cancellationToken)
            {
                IEnumerable<Truck> Trucks = await db.Trucks.ToListAsync(cancellationToken);

                IEnumerable<TruckDto> dto = mapper.Map<IEnumerable<TruckDto>>(Trucks);

                return dto;
            }
        }
    }
}
