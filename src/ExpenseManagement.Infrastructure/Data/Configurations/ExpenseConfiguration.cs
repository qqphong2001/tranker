using ExpenseManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseManagement.Infrastructure.Data.Configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(200);

        builder.Property(e => e.Notes)
            .HasMaxLength(1000);

        builder.Property(e => e.Date)
            .IsRequired();

        builder.HasOne(e => e.Category)
            .WithMany(c => c.Expenses)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Currency)
            .WithMany()
            .HasForeignKey(e => e.CurrencyId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(e => e.Tags)
            .WithMany(t => t.Expenses)
            .UsingEntity(j => j.ToTable("ExpenseTags"));

        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.Date);
    }
}
