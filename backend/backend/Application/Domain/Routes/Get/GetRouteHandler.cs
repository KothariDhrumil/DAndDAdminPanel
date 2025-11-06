using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using SharedKernel;

namespace Application.Domain.Routes.Get;

public sealed class GetRouteHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetRouteQuery, List<GetRouteResponse>>
{
    public async Task<Result<List<GetRouteResponse>>> Handle(GetRouteQuery query, CancellationToken ct)
    {
        var routes = await unitOfWork.Routes.FindAsync(x => x.IsActive, ct);
        
        var response = routes.Select(x => new GetRouteResponse
        {
            Name = x.Name,
            Id = x.Id,
        }).ToList();
        
        return Result.Success(response);
    }
}
