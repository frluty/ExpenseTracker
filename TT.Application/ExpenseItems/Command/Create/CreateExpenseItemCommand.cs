using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Http;
using TT.Application.Common.Enums;
using TT.Application.Common.Interfaces;
using TT.Domain.Entities;
using ValidationException = TT.Application.Common.Exceptions.ValidationException;

namespace TT.Application.ExpenseItems.Command.Create;

public class CreateExpenseItemCommand : IRequest<Unit>
{
    public int UserId { get; set; }
    public string Nature { get; set; } = "Restaurant";
    public float Amount { get; set; }
    [Required] public string? Comment { get; set; }
    [Required] public DateTime Date { get; set; }
}

public class CreateExpenseItemCommandHandler : IRequestHandler<CreateExpenseItemCommand, Unit>
{
    private readonly IAppDbContext _dbContext;

    public CreateExpenseItemCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(CreateExpenseItemCommand request, CancellationToken cancellationToken)
    {
        if (request.Nature is null|| !Enum.IsDefined(typeof(Nature), request.Nature.ToUpper()))
             throw new ValidationException("Expense Nature possible values are: (Restaurant, Hotel, Misc)");
       
        if (request.Date.Date > DateTime.Today || request.Date < DateTime.Today.AddDays(-90)) throw new Exception($"The Expense date is not valid {request.Date}");

        if (request.Amount <= 0)
            throw new Exception("the Amount should > 0");
        
        var user = await _dbContext.Users!.FindAsync(new object?[] { request.UserId, cancellationToken }, cancellationToken);
        if (user is null) throw new Exception($"User {request.UserId} not exist");

        var userExpense = _dbContext.Expenses!.Where(x => x.User == user && x.Date == request.Date.Date && Math.Abs(x.Amount - request.Amount) == 0);
        if (userExpense.Any()) throw new Exception($"User has already an expenses: {request.Amount} , {request.Date}");

        var expense = new Expense
        {
            User = user,
            Amount = request.Amount,
            Currency = user.Currency,
            Date = request.Date.Date,
            Comment = request.Comment,
            Nature = request.Nature
        };
        _dbContext.Expenses!.Add(expense);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}