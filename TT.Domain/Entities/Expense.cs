using System.Runtime.CompilerServices;

namespace TT.Domain.Entities;

public class Expense
{
    public int Id { get; set; }
    public string? Nature { get; set; }
    public float Amount { get; set; }
    public string? Currency { get; set; }
    public string? Comment { get; set; }
    public User User { get; set; }
    public DateTime Date { get; set; }
}