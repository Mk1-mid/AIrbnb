using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Infrastructure.Persistence.Configurations;

public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(2000);
        builder.Property(p => p.City).HasMaxLength(100).IsRequired();
        builder.Property(p => p.IsActive).HasDefaultValue(true);

        builder.OwnsOne(p => p.PricePerNight, money =>
        {
            money.Property(m => m.Amount)
                 .HasColumnName("PriceAmount")
                 .HasPrecision(18, 2)
                 .IsRequired();
            money.Property(m => m.Currency)
                 .HasColumnName("PriceCurrency")
                 .HasMaxLength(3)
                 .HasDefaultValue("COP");
        });

        builder.HasOne(p => p.Owner)
               .WithMany()
               .HasForeignKey(p => p.OwnerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Images)
               .WithOne(i => i.Property)
               .HasForeignKey(i => i.PropertyId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
