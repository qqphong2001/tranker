using ExpenseManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseManagement.Infrastructure.Data.Configurations;

public class RecurringTransactionConfiguration : IEntityTypeConfiguration<RecurringTransaction>
{
    public void Configure(EntityTypeBuilder<RecurringTransaction> builder)
    {
        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(rt => rt.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(rt => rt.Description)
            .HasMaxLength(1000);

        builder.Property(rt => rt.RecurrenceType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(rt => rt.TransactionType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(rt => rt.StartDate)
            .IsRequired();

        builder.Property(rt => rt.NextOccurrence)
            .IsRequired();

        builder.Property(rt => rt.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(rt => rt.Category)
            .WithMany()
            .HasForeignKey(rt => rt.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(rt => rt.Currency)
            .WithMany()
            .HasForeignKey(rt => rt.CurrencyId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(rt => rt.UserId);
        builder.HasIndex(rt => rt.NextOccurrence);
        builder.HasIndex(rt => new { rt.UserId, rt.IsActive });
    }
}
