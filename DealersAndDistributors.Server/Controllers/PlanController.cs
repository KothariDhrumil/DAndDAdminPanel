using Application.Abstractions.Messaging;
using Application.Plans.Create;
using Application.Plans.Delete;
using Application.Plans.Get;
using Application.Plans.Update;
using DealersAndDistributors.Server.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using System.Threading.Tasks;
using Application.Plans.GetById;


namespace DealersAndDistributors.Server.Controllers
{

    public class PlanController : VersionNeutralApiController
    {

        /// <summary>
        /// Gets the list of plans.
        /// </summary>
        /// <param name="handler">The query handler for getting plans.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A list of todo responses wrapped in an <see cref="IResult"/>.</returns>
        [HttpGet]
        public async Task<IResult> Get(IQueryHandler<Application.Plans.Get.GetPlanQuery, List<Application.Plans.Get.PlanResponse>> handler, CancellationToken cancellationToken)
        {
            var query = new Application.Plans.Get.GetPlanQuery();
            Result<List<Application.Plans.Get.PlanResponse>> resonse = await handler.Handle(query, cancellationToken);
            return Results.Ok(resonse);
        }

        ///// <summary>
        ///// Get a plan by id with permissions.
        ///// </summary>
        //[HttpGet("{id:int}")]
        //public async Task<IResult> GetById(int id, IQueryHandler<GetPlanByIdQuery, Application.Plans.GetById.PlanDetailsResponse> handler, CancellationToken cancellationToken)
        //{
        //    var query = new GetPlanByIdQuery(id);
        //    var response = await handler.Handle(query, cancellationToken);
        //    return Results.Ok(response);
        //}

        /// <summary>
        /// Creates a new plan item.
        /// </summary>

        [HttpPost]
        public async Task<IResult> Create([FromBody] CreatePlanCommand command,
            ICommandHandler<CreatePlanCommand, int> handler,
            CancellationToken cancellationToken)
        {
            var response = await handler.Handle(command, cancellationToken);
            return Results.Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IResult> Post([FromBody] UpdatePlanCommand command,
            ICommandHandler<UpdatePlanCommand, int> handler,
            CancellationToken cancellationToken)
        {
            var response = await handler.Handle(command, cancellationToken);
            return Results.Ok(response);
        }

        // DELETE /api/plan?planId=123
        [HttpDelete]
        [Authorize]
        public async Task<IResult> Delete([FromQuery] int Id,
            ICommandHandler<DeletePlanCommand> handler,
            CancellationToken cancellationToken)
        {
            var response = await handler.Handle(new DeletePlanCommand(Id), cancellationToken);
            return Results.Ok(response);
        }


    }
}
