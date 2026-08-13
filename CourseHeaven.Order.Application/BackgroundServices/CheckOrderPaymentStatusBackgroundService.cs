using CourseHeaven.Order.Application.Contracts.Refit.PaymentService;
using CourseHeaven.Order.Application.Contracts.Repositories;
using CourseHeaven.Order.Application.Contracts.UnitOfWork;
using CourseHeaven.Order.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CourseHeaven.Order.Application.BackgroundServices;

public class CheckOrderPaymentStatusBackgroundService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();

        var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
        var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var orders = orderRepository.Where(o => o.Status == OrderStatus.WaitingForPayment).ToList();

            foreach (var order in orders)
            {
                var paymentStatusResponse = await paymentService.GetStatusAsync(order.OrderCode);

                if (paymentStatusResponse.IsPaid)
                {
                    await orderRepository.SetStatusAsync(order.OrderCode, paymentStatusResponse.PaymentId!.Value,
                        OrderStatus.Paid, stoppingToken);
                    await unitOfWork.CommitAsync(stoppingToken);
                }
            }

            await Task.Delay(2000, stoppingToken);
        }
    }
}