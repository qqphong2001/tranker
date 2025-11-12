using ExpenseManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseManagement.Infrastructure.Data.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(b => b.SpentAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(b => b.WarningThreshold)
            .HasPrecision(5, 2);

        builder.HasOne(b => b.Category)
            .WithMany(c => c.Budgets)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(b => b.UserId);
        builder.HasIndex(b => new { b.StartDate, b.EndDate });
    }
}
