using TT.Domain.Entities;

namespace TT.Application.ExpenseItems.Dtos;

public record ExpenseDto(int Id, string User, string Nature, float Amount, string Currency, string Comment, DateTime Date)
{
    public static ExpenseDto FromExpenseEntity(Expense expense) =>
        new(expense.Id,$"{expense.User.FirstName} {expense.User.LastName}" , expense.Nature!, expense.Amount, expense.Currency!, expense.Comment!, expense.Date);
}