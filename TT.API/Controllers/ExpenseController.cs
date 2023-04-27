using MediatR;
using Microsoft.AspNetCore.Mvc;
using TT.Application.Common.Models;
using TT.Application.ExpenseItems.Command.Create;
using TT.Application.ExpenseItems.Dtos;
using TT.Application.ExpenseItems.Queries;

namespace TT.API.Controllers;

public class ExpenseController : ApiBaseController
{
    [HttpPost("create")]
    public async Task<ActionResult<Unit>> Create(CreateExpenseItemCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("list")]
    public async Task<ActionResult<PaginatedList<ExpenseDto>>> GetExpenseItemsWithPagination([FromQuery] GetExpensePaginatedListQuery query)
    {
        return await Mediator.Send(query);
    }
}