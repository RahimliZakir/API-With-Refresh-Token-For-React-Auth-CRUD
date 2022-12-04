using Application.WebAPI.Models.DataContexts;
using MediatR.Pipeline;

namespace Application.WebAPI.AppCode.Application.Behaviours
{
    public class PreCommandBehaviour<T> : IRequestPreProcessor<T>
        where T : notnull
    {
        readonly VehicleDbContext db;

        public PreCommandBehaviour(VehicleDbContext db)
        {
            this.db = db;
        }

        public async Task Process(T request, CancellationToken cancellationToken)
        {
            await db.Database.BeginTransactionAsync(cancellationToken);
        }
    }
}
