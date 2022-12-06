using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Mappers.Dtos;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WebAPI.AppCode.Application.Modules.CarModule
{
    public class CarSingleQuery : IRequest<CommandJsonResponse>
    {
        public int? Id { get; set; }

        public class CarSingleQueryHandler : IRequestHandler<CarSingleQuery, CommandJsonResponse>
        {
            readonly VehicleDbContext db;
            readonly IMapper mapper;

            public CarSingleQueryHandler(VehicleDbContext db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public async Task<CommandJsonResponse> Handle(CarSingleQuery request, CancellationToken cancellationToken)
            {
                if (request.Id == null || request.Id <= 0)
                    return new CommandJsonResponse("Id düzgün göndərilməyib!", true);

                Car? car = await db.Cars
                                   .Include(c => c.CarImages)
                                   .FirstOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

                if (car is null)
                    return new CommandJsonResponse("Məlumat tapılmadı!", true);

                CarDto dto = mapper.Map<CarDto>(car);

                return new CommandJsonResponse<CarDto>("Uğrulu!", false, dto);
            }
        }
    }
}
