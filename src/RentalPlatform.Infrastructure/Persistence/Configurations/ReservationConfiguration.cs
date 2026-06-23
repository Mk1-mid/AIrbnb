using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Infrastructure.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(r => r.Id);

        builder.OwnsOne(r => r.StayPeriod, period =>
        {
            period.Property(p => p.CheckIn)
                  .HasColumnName("CheckIn")
                  .IsRequired();
            period.Property(p => p.CheckOut)
                  .HasColumnName("CheckOut")
                  .IsRequired();
        });

        builder.OwnsOne(r => r.TotalPrice, money =>
        {
            money.Property(m => m.Amount)
                 .HasColumnName("TotalAmount")
                 .HasPrecision(18, 2)
                 .IsRequired();
            money.Property(m => m.Currency)
                 .HasColumnName("TotalCurrency")
                 .HasMaxLength(3)
                 .HasDefaultValue("COP");
        });

        builder.Property(r => r.Status).HasConversion<string>();
        builder.Property(r => r.CreatedAt).IsRequired();

        builder.HasOne(r => r.Property)
               .WithMany(p => p.Reservations)
               .HasForeignKey(r => r.PropertyId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Guest)
               .WithMany(u => u.Reservations)
               .HasForeignKey(r => r.GuestId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
