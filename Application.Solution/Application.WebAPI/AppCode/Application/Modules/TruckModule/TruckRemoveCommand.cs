using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Application.Modules.TruckModule;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WebAPI.AppCode.Application.Modules.TruckModule
{
    public class TruckRemoveCommand : IRequest<CommandJsonResponse>
    {
        public int? Id { get; set; }

        public class TruckRemoveCommandHandler : IRequestHandler<TruckRemoveCommand, CommandJsonResponse>
        {
            readonly VehicleDbContext db;

            public TruckRemoveCommandHandler(VehicleDbContext db)
            {
                this.db = db;
            }

            async public Task<CommandJsonResponse> Handle(TruckRemoveCommand request, CancellationToken cancellationToken)
            {
                if (request.Id == null || request.Id <= 0)
                {
                    return new CommandJsonResponse("Məlumat tamlığı qorunmayıb!", true);
                }

                Truck? entity = await db.Trucks.FirstOrDefaultAsync(p => p.Id.Equals(request.Id) && p.DeletedDate == null, cancellationToken);

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
