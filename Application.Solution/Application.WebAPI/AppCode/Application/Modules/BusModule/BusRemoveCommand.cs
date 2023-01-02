using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Application.Modules.BusModule;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WebAPI.AppCode.Application.Modules.BusModule
{
    public class BusRemoveCommand : IRequest<CommandJsonResponse>
    {
        public int? Id { get; set; }

        public class BusRemoveCommandHandler : IRequestHandler<BusRemoveCommand, CommandJsonResponse>
        {
            readonly VehicleDbContext db;

            public BusRemoveCommandHandler(VehicleDbContext db)
            {
                this.db = db;
            }

            async public Task<CommandJsonResponse> Handle(BusRemoveCommand request, CancellationToken cancellationToken)
            {
                if (request.Id == null || request.Id <= 0)
                {
                    return new CommandJsonResponse("Məlumat tamlığı qorunmayıb!", true);
                }

                Bus? entity = await db.Buses.FirstOrDefaultAsync(p => p.Id.Equals(request.Id) && p.DeletedDate == null, cancellationToken);

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
