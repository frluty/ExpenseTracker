using Microsoft.EntityFrameworkCore;
using TT.Domain.Entities;

namespace TT.Application.Common.Interfaces;

public interface IAppDbContext
{
    public DbSet<Expense>? Expenses {get; set; }
    public DbSet<User>? Users {get; set; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);

}