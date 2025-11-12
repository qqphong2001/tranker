using ExpenseManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseManagement.Infrastructure.Data.Configurations;

public class IncomeConfiguration : IEntityTypeConfiguration<Income>
{
    public void Configure(EntityTypeBuilder<Income> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(i => i.Description)
            .HasMaxLength(200);

        builder.Property(i => i.Notes)
            .HasMaxLength(1000);

        builder.Property(i => i.Source)
            .HasMaxLength(100);

        builder.Property(i => i.Date)
            .IsRequired();

        builder.HasOne(i => i.Category)
            .WithMany(c => c.Incomes)
            .HasForeignKey(i => i.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Currency)
            .WithMany()
            .HasForeignKey(i => i.CurrencyId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(i => i.UserId);
        builder.HasIndex(i => i.Date);
    }
}
