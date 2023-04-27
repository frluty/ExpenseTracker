using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Moq;
using TT.Application.Common.Enums;
using TT.Application.Common.Exceptions;
using TT.Application.Common.Extensions;
using TT.Application.Common.Interfaces;
using TT.Application.ExpenseItems.Command.Create;
using TT.Domain.Entities;
using Xunit;

namespace TT.UnitTest;


public class CreateExpenseItemCommandHandlerTests
{
    private readonly Mock<IAppDbContext> _dbContextMock;
    private readonly CreateExpenseItemCommandHandler _handler;

    public CreateExpenseItemCommandHandlerTests()
    {
        _dbContextMock = new Mock<IAppDbContext>();
        _handler = new CreateExpenseItemCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_CreatesExpenseItem()
    {
        const int userId = 1;
        const float amount = 10.0f;
        const string comment = "Test expense";
        var date = DateTime.Today.AddDays(-1);
        var request = new CreateExpenseItemCommand
        {
            UserId = userId,
            Amount = amount,
            Comment = comment,
            Date = date,
            Nature = "Restaurant"
        };
        var user = new User { Id = userId, Currency = Currencies.USD.GetEnumDescription() };
        _dbContextMock.Setup(x => x.Users!.FindAsync(new object[] { userId }, default)).ReturnsAsync(user);
        _dbContextMock.Setup(x => x.Expenses!.Add(It.IsAny<Expense>()));
        _dbContextMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

        var result = await _handler.Handle(request, default);

        _dbContextMock.Verify(x => x.Expenses!.Add(It.Is<Expense>(e =>
            e.User == user &&
            Math.Abs(e.Amount - amount) == 0 &&
            e.Comment == comment &&
            e.Date == date.Date &&
            e.Nature == request.Nature &&
            e.Currency == user.Currency
        )), Times.Once);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_WithNonExistingUser_ThrowsException()
    {
        const int userId = 1;
        var request = new CreateExpenseItemCommand
        {
            UserId = userId,
            Amount = 10.0f,
            Comment = "Test expense",
            Date = DateTime.Today.AddDays(-1),
            Nature = "Restaurant"
        };
        _dbContextMock.Setup(x => x.Users!.FindAsync(new object[] { userId }, default)).ReturnsAsync((User)null);

        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, default));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10.0f)]
    public async Task Handle_WithInvalidAmount_ThrowsException(float amount)
    {
        var request = new CreateExpenseItemCommand
        {
            UserId = 1,
            Amount = amount,
            Comment = "Test expense",
            Date = DateTime.Today.AddDays(-1),
            Nature = "Restaurant"
        };

        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, default));
    }

    [Theory]
    [InlineData("InvalidNature")]
    [InlineData(null)]
    public async Task Handle_WithInvalidNature_ThrowsException(string nature)
    {
        var request = new CreateExpenseItemCommand
        {
            UserId = 1,
            Amount = 10.0f,
            Comment = "Test expense",
            Date = DateTime.Today.AddDays(-1),
            Nature = nature
        };

        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, default));
    }

    [Theory]
    [InlineData("Restaurant")]
    [InlineData("Hotel")]
    [InlineData("Misc")]
    public async Task Handle_WithValidNature_DoesNotThrowException(string nature)
    {
        const int userId = 1;
        var amount = (float)(new Random().NextDouble() * 99.0f) + 1.0f;

        const string comment = "Test expense";
        var date = DateTime.Today.AddDays(-1);
        var request = new CreateExpenseItemCommand
        {
            UserId = userId,
            Amount = amount,
            Comment = comment,
            Date = date,
            Nature = nature
        };
        var user = new User { Id = userId, Currency = Currencies.USD.GetEnumDescription() };
        _dbContextMock.Setup(x => x.Users!.FindAsync(new object[] { userId }, default)).ReturnsAsync(user);

        _dbContextMock.Setup(x => x.Expenses!
                .FirstOrDefault(e => e.User == user && e.Date == date.Date && e.Amount - amount == 0))
            .Returns((Expense)null!);

        var result = await _handler.Handle(request, default);
        Assert.Equal(Unit.Value, result);
    }
}