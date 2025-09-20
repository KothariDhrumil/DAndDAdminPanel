using Application.Abstractions.Messaging;
using Application.TenantPlans.Create;
using Application.TenantPlans.Delete;
using Application.TenantPlans.Get;
using Application.TenantPlans.GetActivePlan;
using Application.TenantPlans.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace DealersAndDistributors.Server.Controllers
{
    public class TenantPlanController : VersionNeutralApiController
    {
        // GET: list of active tenant plans
        [HttpGet]
        public async Task<IResult> Get(
            IQueryHandler<GetTenantPlanQuery, List<TenantPlanInfo>> handler,
            CancellationToken cancellationToken)
        {
            var query = new GetTenantPlanQuery();
            var response = await handler.Handle(query, cancellationToken);
            return Results.Ok(response);
        }

        // GET: active plan for a specific tenant
        [HttpGet("active/{tenantId:int}")]
        public async Task<IResult> GetActiveByTenantId(
            int tenantId,
            IQueryHandler<GetActiveTenantPlanByIdQuery, ActiveTenentPlanResponse> handler,
            CancellationToken cancellationToken)
        {
            var query = new GetActiveTenantPlanByIdQuery(tenantId);
            var response = await handler.Handle(query, cancellationToken);
            return Results.Ok(response);
        }

        // POST: create a tenant plan
        [HttpPost]
        [Authorize]
        public async Task<IResult> Create(
            [FromBody] CreateTenantPlanCommand command,
            ICommandHandler<CreateTenantPlanCommand, int> handler,
            CancellationToken cancellationToken)
        {
            var response = await handler.Handle(command, cancellationToken);
            return Results.Ok(response);
        }

        // PUT: update a tenant plan
        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IResult> Update(
            int id,
            [FromBody] UpdateTenantPlanCommand command,
            ICommandHandler<UpdateTenantPlanCommand> handler,
            CancellationToken cancellationToken)
        {
            // Ensure route id is applied if body is missing it
            if (command.TenantPlanId == 0)
                command.TenantPlanId = id;

            var response = await handler.Handle(command, cancellationToken);
            return Results.Ok(response);
        }

        // DELETE: delete tenant plan via query parameter ?tenantPlanId=123
        [HttpDelete]
        [Authorize]
        public async Task<IResult> Delete(
            [FromQuery] int tenantPlanId,
            ICommandHandler<DeleteTenantPlanCommand> handler,
            CancellationToken cancellationToken)
        {
            var response = await handler.Handle(new DeleteTenantPlanCommand(tenantPlanId), cancellationToken);
            return Results.Ok(response);
        }
    }
}
