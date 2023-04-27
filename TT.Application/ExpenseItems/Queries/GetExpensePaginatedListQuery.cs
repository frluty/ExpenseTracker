using System.ComponentModel.DataAnnotations;
using MediatR;
using TT.Application.Common.Enums;
using TT.Application.Common.Interfaces;
using TT.Application.Common.Models;
using TT.Application.ExpenseItems.Dtos;

namespace TT.Application.ExpenseItems.Queries;

public class GetExpensePaginatedListQuery : IRequest<PaginatedList<ExpenseDto>>
{
    [Required]
    public int UserId { get; set; }
    public SortBy SortBy { get; set; }
    public SortOrder SortOrder { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetExpensePaginatedListQueryHandler : IRequestHandler<GetExpensePaginatedListQuery, PaginatedList<ExpenseDto>>
{
    private readonly IAppDbContext _dbContext;

    public GetExpensePaginatedListQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginatedList<ExpenseDto>> Handle(GetExpensePaginatedListQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users!.FindAsync(new object?[] { request.UserId, cancellationToken }, cancellationToken);
        if (user is null) throw new Exception($"User {request.UserId} not exist");

        var queryable = _dbContext.Expenses!.Where(x => x.User == user);

        queryable = request.SortBy switch
        {
            SortBy.AMOUNT => request.SortOrder == SortOrder.DESC ? queryable.OrderByDescending(e => e.Amount) : queryable.OrderBy(e => e.Amount),
            SortBy.DATE => request.SortOrder == SortOrder.DESC ? queryable.OrderByDescending(e => e.Date) : queryable.OrderBy(e => e.Date),
            _ => queryable
        };

        return await PaginatedList<ExpenseDto>.CreateAsync(queryable.Select(x => ExpenseDto.FromExpenseEntity(x)), request.PageNumber, request.PageSize);
    }
}