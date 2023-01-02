using Application.WebAPI.AppCode.Application.Modules.BusModule;
using Application.WebAPI.AppCode.Mappers.Dtos;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WebAPI.AppCode.Application.Modules.BusModule
{
    public class BusGetAllActiveQuery : IRequest<IEnumerable<BusDto>>
    {
        public class BusGetAllActiveQueryHandler : IRequestHandler<BusGetAllActiveQuery, IEnumerable<BusDto>>
        {
            readonly VehicleDbContext db;
            readonly IMapper mapper;

            public BusGetAllActiveQueryHandler(VehicleDbContext db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            async public Task<IEnumerable<BusDto>> Handle(BusGetAllActiveQuery request, CancellationToken cancellationToken)
            {
                IEnumerable<Bus> buses = await db.Buses.Where(b => b.DeletedDate == null).ToListAsync(cancellationToken);

                IEnumerable<BusDto> dto = mapper.Map<IEnumerable<BusDto>>(buses);

                return dto;
            }
        }
    }
}
