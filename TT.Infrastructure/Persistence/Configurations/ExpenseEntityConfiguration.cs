using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TT.Domain.Entities;

namespace TT.Infrastructure.Persistence.Configurations;

public class ExpenseEntityConfiguration:IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("Expenses");
        // builder
        //     .HasOne(e => e.User)
        //     .WithMany()
        //     .HasForeignKey(e => e.User);
    }
}