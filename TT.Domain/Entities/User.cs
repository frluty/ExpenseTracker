
namespace TT.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Currency { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public IList<Expense> Expenses { get; set; } = new List<Expense>(); 
}