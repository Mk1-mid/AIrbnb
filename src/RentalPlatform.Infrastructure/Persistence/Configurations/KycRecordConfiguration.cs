using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Infrastructure.Persistence.Configurations;

public class KycRecordConfiguration : IEntityTypeConfiguration<KycRecord>
{
    public void Configure(EntityTypeBuilder<KycRecord> builder)
    {
        builder.HasKey(k => k.Id);
        builder.Property(k => k.DocumentNumber).HasMaxLength(20).IsRequired();
        builder.Property(k => k.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(k => k.LastName).HasMaxLength(100).IsRequired();
        builder.Property(k => k.Status).HasConversion<string>();
    }
}
