using MediatR;
using TT.Application.Common.Exceptions;
using TT.Application.Common.Interfaces;
using TT.Application.UserItems.Dtos;
using TT.Domain.Entities;

namespace TT.Application.UserItems.Queries;

public class GetUserByIdQuery:IRequest<UserDto>
{
    public GetUserByIdQuery(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}
public class GetUserByIdQueryHandler:IRequestHandler< GetUserByIdQuery, UserDto>
{
    private readonly IAppDbContext _dbContext;

    public GetUserByIdQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users!.FindAsync(new object?[] { request.Id, cancellationToken }, cancellationToken: cancellationToken);
        if (user is null)
            throw new NotFoundException($"Use",request.Id);

        return UserDto.FromUserEntity(user);
    }
}
