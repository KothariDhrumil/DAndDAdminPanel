using Application.Abstractions.Messaging;
using Application.Domain.UserTypes.Get;
using Application.Domain.UserTypes.GetById;
using Application.Domain.UserTypes.Create;
using Application.Domain.UserTypes.Update;
using Application.Domain.UserTypes.Delete;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace DealersAndDistributors.Server.Controllers.Domain;

public class UserTypesController : VersionedApiController
{
    [HttpGet]
    public async Task<IActionResult> GetUserTypesAsync(
        IQueryHandler<GetUserTypesQuery, List<GetUserTypeResponseDTO>> handler,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.Handle(new GetUserTypesQuery(), cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserTypeByIdAsync(int id,
        IQueryHandler<GetUserTypeByIdQuery, GetByIdUserTypeResponseDTO> handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(new GetUserTypeByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserTypeAsync(
        [FromBody] CreateUserTypeCommand command,
        ICommandHandler<CreateUserTypeCommand, int> handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(command, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUserTypeAsync(int id,
        [FromBody] UpdateUserTypeCommand command,
        ICommandHandler<UpdateUserTypeCommand> handler,
        CancellationToken cancellationToken)
    {
        command.UserTypeId = id;
        return Ok(await handler.Handle(command, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUserTypeAsync(int id,
        ICommandHandler<DeleteUserTypeCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new DeleteUserTypeCommand { UserTypeId = id }, cancellationToken);
        return Ok(result);
    }
}
