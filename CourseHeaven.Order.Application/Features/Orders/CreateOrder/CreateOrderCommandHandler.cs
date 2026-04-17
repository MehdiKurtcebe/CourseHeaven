using CourseHeaven.Order.Application.Contracts.Repositories;
using CourseHeaven.Order.Application.Contracts.UnitOfWork;
using CourseHeaven.Shared;
using CourseHeaven.Shared.Services;
using MediatR;

namespace CourseHeaven.Order.Application.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IIdentityService identityService,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // if (request.OrderItems.Count == 0)
        // {
        //     return ServiceResult.Error("Order must contain at least one item.", "Order must contain at least one item.", HttpStatusCode.BadRequest);
        // }

        var order = Domain.Entities.Order.GenerateUnpaidOrder(identityService.UserId, request.InvoiceAddress,
            request.DiscountRate);
        foreach (var orderItem in request.OrderItems)
            order.AddOrderItem(orderItem.ProductId, orderItem.ProductName, orderItem.ProductPrice);

        orderRepository.Add(order);
        await unitOfWork.CommitAsync(cancellationToken);

        var paymentId = Guid.Empty;
        // TODO: Payment process
        order.SetStatusToPaid(paymentId);

        orderRepository.Update(order);
        await unitOfWork.CommitAsync(cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}