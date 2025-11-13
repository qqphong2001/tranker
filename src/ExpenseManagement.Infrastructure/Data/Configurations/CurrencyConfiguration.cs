using ExpenseManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseManagement.Infrastructure.Data.Configurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(3); // ISO 4217 currency codes are 3 characters

        builder.Property(c => c.Symbol)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.ExchangeRate)
            .HasPrecision(18, 6)
            .HasDefaultValue(1.0m);

        builder.Property(c => c.IsDefault)
            .HasDefaultValue(false);

        builder.HasIndex(c => c.Code)
            .IsUnique();

        builder.HasIndex(c => c.IsDefault);
    }
}
