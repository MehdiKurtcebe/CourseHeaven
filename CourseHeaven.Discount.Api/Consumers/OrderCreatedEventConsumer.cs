using CourseHeaven.Bus.Events;
using CourseHeaven.Discount.Api.Features.Discounts;
using CourseHeaven.Discount.Api.Repositories;
using MassTransit;

namespace CourseHeaven.Discount.Api.Consumers;

public class OrderCreatedEventConsumer(IServiceProvider serviceProvider) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        using var scope = serviceProvider.CreateScope();
        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // TODO: Review this logic later
        var discount = new Features.Discounts.Discount()
        {
            Id = NewId.NextSequentialGuid(),
            UserId = context.Message.UserId,
            DiscountRate = 0.1m,
            CouponCode = DiscountCodeGenerator.GenerateDiscountCode(10),
            CreatedAt = DateTimeOffset.UtcNow,
            ExpireAt = DateTimeOffset.UtcNow.AddMonths(1),
        };
        
        await appDbContext.Discounts.AddAsync(discount);
        await appDbContext.SaveChangesAsync();
    }
}