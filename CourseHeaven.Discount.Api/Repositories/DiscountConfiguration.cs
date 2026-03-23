using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CourseHeaven.Discount.Api.Repositories;

public class DiscountConfiguration : IEntityTypeConfiguration<Features.Discounts.Discount>
{
    public void Configure(EntityTypeBuilder<Features.Discounts.Discount> builder)
    {
        builder.ToCollection("discounts");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasElementName("_id").ValueGeneratedNever();
        builder.Property(d => d.UserId).HasElementName("user_id");
        builder.Property(d => d.DiscountRate).HasElementName("discount_rate").HasPrecision(3, 3);
        builder.Property(d => d.CouponCode).HasElementName("coupon_code").HasMaxLength(100);
        builder.Property(d => d.CreatedAt).HasElementName("created_at");
        builder.Property(d => d.UpdatedAt).HasElementName("updated_at");
        builder.Property(d => d.ExpireAt).HasElementName("expire_at");
    }
}