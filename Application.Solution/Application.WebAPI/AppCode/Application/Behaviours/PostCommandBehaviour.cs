using Application.WebAPI.Models.DataContexts;
using MediatR;
using MediatR.Pipeline;

namespace Application.WebAPI.AppCode.Application.Behaviours
{
    public class PostCommandBehaviour<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        readonly VehicleDbContext db;

        public PostCommandBehaviour(VehicleDbContext db)
        {
            this.db = db;
        }

        async public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            if (db.Database.CurrentTransaction is not null)
                await db.Database.CurrentTransaction.CommitAsync(cancellationToken);
        }
    }
}
