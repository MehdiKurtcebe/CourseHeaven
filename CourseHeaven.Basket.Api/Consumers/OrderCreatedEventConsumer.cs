using CourseHeaven.Basket.Api.Features.Baskets;
using CourseHeaven.Bus.Events;
using MassTransit;

namespace CourseHeaven.Basket.Api.Consumers;

public class OrderCreatedEventConsumer(IServiceProvider serviceProvider) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        using var scope = serviceProvider.CreateScope();
        var basketService = scope.ServiceProvider.GetRequiredService<BasketService>();
        await basketService.DeleteBasketAsync(context.Message.UserId);
    }
}