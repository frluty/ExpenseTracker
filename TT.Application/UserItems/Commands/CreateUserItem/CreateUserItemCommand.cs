using System.ComponentModel.DataAnnotations;
using MediatR;
using TT.Application.Common.Enums;
using TT.Application.Common.Extensions;
using TT.Application.Common.Interfaces;
using TT.Domain.Entities;

namespace TT.Application.UserItems.Commands.CreateUserItem;

public class CreateUserItemCommand : IRequest<Unit>
{
    [Required] public string? FirstName { get; set; }
    [Required] public string? LastName { get; set; }
    [Required] public string Currency { get; set; } = "EUR";
}

public class CreateUserItemCommandHandler : IRequestHandler<CreateUserItemCommand, Unit>
{
    private readonly IAppDbContext _dbContext;

    public CreateUserItemCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(CreateUserItemCommand request, CancellationToken cancellationToken)
    {
        if (Enum.TryParse(request.Currency, out Currencies currency))
            request.Currency = currency.GetEnumDescription();
        var user = new User
        {
            LastName = request.LastName,
            FirstName = request.FirstName,
            Currency = request.Currency,
            Created = DateTime.Now
        };
        _dbContext.Users!.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}