using MediatR;
using TT.Application.Common.Interfaces;
using TT.Application.Common.Models;
using TT.Application.UserItems.Dtos;
using TT.Domain.Entities;

namespace TT.Application.UserItems.Queries;

public class GetUserListWithPaginatedQuery:IRequest<PaginatedList<UserDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetUserListWithPaginatedQueryHandler:IRequestHandler<GetUserListWithPaginatedQuery, PaginatedList<UserDto>>
{
    private readonly IAppDbContext _dbContext;

    public GetUserListWithPaginatedQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginatedList<UserDto>> Handle(GetUserListWithPaginatedQuery request, CancellationToken cancellationToken)
    {

        return await PaginatedList<UserDto>.CreateAsync(_dbContext.Users!.Select(x=> UserDto.FromUserEntity(x)), request.PageNumber, request.PageSize);
    }
}
