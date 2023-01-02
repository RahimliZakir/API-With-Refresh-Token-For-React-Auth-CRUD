using Application.WebAPI.AppCode.Mappers.Dtos;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WebAPI.AppCode.Application.Modules.CarModule
{
    public class CarGetAllActiveQuery : IRequest<IEnumerable<CarDto>>
    {
        public class CarGetAllActiveQueryHandler : IRequestHandler<CarGetAllActiveQuery, IEnumerable<CarDto>>
        {
            readonly VehicleDbContext db;
            readonly IMapper mapper;

            public CarGetAllActiveQueryHandler(VehicleDbContext db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            async public Task<IEnumerable<CarDto>> Handle(CarGetAllActiveQuery request, CancellationToken cancellationToken)
            {
                IEnumerable<Car> cars = await db.Cars.Include(c => c.CarImages).Where(p => p.DeletedDate == null).ToListAsync(cancellationToken);

                IEnumerable<CarDto> dto = mapper.Map<IEnumerable<CarDto>>(cars);

                return dto;
            }
        }
    }
}
