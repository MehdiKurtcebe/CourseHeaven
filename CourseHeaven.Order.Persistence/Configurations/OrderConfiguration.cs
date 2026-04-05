using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseHeaven.Order.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Domain.Entities.Order>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id)
            .ValueGeneratedNever();

        builder.Property(o => o.OrderCode)
            .IsRequired();

        builder.Property(oi => oi.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(oi => oi.BuyerId)
            .IsRequired();

        builder.Property(o => o.TotalPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(o => o.DiscountRate)
            .IsRequired()
            .HasColumnType("decimal(3,3)");

        builder.Property(oi => oi.InvoiceAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(oi => oi.PaymentId);

        builder.Property(oi => oi.CreatedAt)
            .IsRequired();

        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}