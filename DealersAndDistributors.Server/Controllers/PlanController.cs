using Application.Abstractions.Messaging;
using Application.Plans.Create;
using Application.Plans.Delete;
using Application.Plans.Get;
using Application.Plans.Update;
using Application.Todos.Create;
using Application.Todos.Get;
using DealersAndDistributors.Server.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using System.Threading.Tasks;


namespace DealersAndDistributors.Server.Controllers
{
    public class PlanController : VersionedApiController
    {

        /// <summary>
        /// Gets the list of todos.
        /// </summary>
        /// <param name="handler">The query handler for getting todos.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A list of todo responses wrapped in an <see cref="IResult"/>.</returns>
        [HttpGet]
        public async Task<IResult> Get(IQueryHandler<GetPlanQuery, List<PlanResponse>> handler, CancellationToken cancellationToken)
        {
            var query = new GetPlanQuery();
            Response<List<PlanResponse>> resonse = await handler.Handle(query, cancellationToken);
            return Results.Ok(resonse);
        }

        /// <summary>
        /// Creates a new todo item.
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


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IResult> Delete([FromBody] DeletePlanCommand command,
            ICommandHandler<DeletePlanCommand, string> handler,
            CancellationToken cancellationToken)
        {
            var response = await handler.Handle(command, cancellationToken);
            return Results.Ok(response);
        }


    }
}
