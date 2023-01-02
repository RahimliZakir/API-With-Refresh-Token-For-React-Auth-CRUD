using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Application.WebAPI.AppCode.Application.Modules.CarModule
{
    public class CarRemoveCommand : IRequest<CommandJsonResponse>
    {
        public int? Id { get; set; }

        public class CarRemoveCommandHandler : IRequestHandler<CarRemoveCommand, CommandJsonResponse>
        {
            readonly VehicleDbContext db;

            public CarRemoveCommandHandler(VehicleDbContext db)
            {
                this.db = db;
            }

            async public Task<CommandJsonResponse> Handle(CarRemoveCommand request, CancellationToken cancellationToken)
            {
                if (request.Id == null || request.Id <= 0)
                {
                    return new CommandJsonResponse("Məlumat tamlığı qorunmayıb!", true);
                }

                Car? entity = await db.Cars
                                      .Include(c => c.CarImages)
                                      .FirstOrDefaultAsync(p => p.Id.Equals(request.Id) && p.DeletedDate == null, cancellationToken);

                if (entity is null)
                {
                    return new CommandJsonResponse("Məlumat mövcud deyil!", true);
                }

                entity.DeletedDate = DateTime.UtcNow.AddHours(4);
                await db.SaveChangesAsync(cancellationToken);

                return new CommandJsonResponse("Məlumat uğurla silindi!", false);
            }
        }
    }
}
