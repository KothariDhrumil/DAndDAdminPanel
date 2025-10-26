using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Routes.GetById;

public sealed class GetCustomerByRouteIdQueryHandler(IRetailDbContext db) : IQueryHandler<GetCustomerByRouteIdQuery, List<GetCustomerByRouteIdResponse>>
{
    public async Task<Result<List<GetCustomerByRouteIdResponse>>> Handle(GetCustomerByRouteIdQuery query, CancellationToken ct)
    {
        var customers = await db.Routes.AsNoTracking()
            .Where(x => x.Id == query.Id)
            .SelectMany(x => x.Customers.Select(y => new GetCustomerByRouteIdResponse()
            {
                Name = y.FirstName + " " + y.LastName,
                TenantUserId = y.TenantUserId,
                SequenceNo = y.SequenceNo
            })).OrderBy(x => x.SequenceNo).ToListAsync();


        return customers == null
            ? Result.Failure<List<GetCustomerByRouteIdResponse>>(Error.NotFound("RouteNotFound", "Route not found."))
            : Result.Success(customers);
    }
}
