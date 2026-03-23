using System.Net;
using CourseHeaven.Discount.Api.Repositories;
using CourseHeaven.Shared.Services;
using MassTransit;

namespace CourseHeaven.Discount.Api.Features.Discounts.CreateDiscount;

public class CreateDiscountCommandHandler(AppDbContext context)
    : IRequestHandler<CreateDiscountCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        var hasCouponCodeForUser =
            await context.Discounts.AnyAsync(d => d.UserId == request.UserId && d.CouponCode == request.CouponCode,
                cancellationToken);
        if (hasCouponCodeForUser)
            return ServiceResult.Error("User already has a discount with the same coupon code.",
                HttpStatusCode.BadRequest);

        var discount = new Discount
        {
            Id = NewId.NextSequentialGuid(),
            UserId = request.UserId,
            DiscountRate = request.DiscountRate,
            CouponCode = request.CouponCode,
            ExpireAt = request.ExpireAt,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await context.Discounts.AddAsync(discount, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}