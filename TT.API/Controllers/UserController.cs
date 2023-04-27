using MediatR;
using Microsoft.AspNetCore.Mvc;
using TT.Application.Common.Models;
using TT.Application.UserItems.Commands.CreateUserItem;
using TT.Application.UserItems.Dtos;
using TT.Application.UserItems.Queries;
using TT.Domain.Entities;

namespace TT.API.Controllers;

public class UserController:ApiBaseController
{
    [HttpPost("create")]
    public async Task<ActionResult<Unit>> Create(CreateUserItemCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpGet("{userId:int}")]
    public async Task<ActionResult<UserDto>> Create( int userId)
    {
        return await Mediator.Send(new GetUserByIdQuery(userId));
    }
    [HttpGet("list")]
    public async Task<ActionResult<PaginatedList<UserDto>>> GetListAsync([FromQuery] GetUserListWithPaginatedQuery query)
    {
        return await Mediator.Send(query);
    }
}