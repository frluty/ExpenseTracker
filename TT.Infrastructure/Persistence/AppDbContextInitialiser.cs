using Microsoft.EntityFrameworkCore;
using TT.Application.Common.Enums;
using TT.Application.Common.Extensions;
using TT.Domain.Entities;

namespace TT.Infrastructure.Persistence;

public class AppDbContextInitialiser
{
    private readonly AppDbContext _dbContext;

    public AppDbContextInitialiser(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InitialiseAsync()
    {
        if (_dbContext.Database.IsSqlServer())
            await _dbContext.Database.MigrateAsync();
    }

   public async Task SeedAsync()
    {
        await TrySeedAsync();
    }

    private async Task TrySeedAsync()
    {
        if (!_dbContext.Users!.Any())
        {
            _dbContext.Users!.Add(new User { FirstName = "Stark", LastName = "Anthony", Currency = Currencies.USD.GetEnumDescription(), Created = DateTime.Today });
            _dbContext.Users!.Add(new User { FirstName = "Romanova", LastName = "Natasha", Currency = Currencies.RUB.GetEnumDescription(), Created = DateTime.Today });
            await _dbContext.SaveChangesAsync();
        }
    }
}