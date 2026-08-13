using System.Net;
using CourseHeaven.Bus.Events;
using CourseHeaven.Order.Application.Contracts.Refit.PaymentService;
using CourseHeaven.Order.Application.Contracts.Repositories;
using CourseHeaven.Order.Application.Contracts.UnitOfWork;
using CourseHeaven.Shared;
using CourseHeaven.Shared.Services;
using MassTransit;
using MediatR;

namespace CourseHeaven.Order.Application.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IIdentityService identityService,
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint,
    IPaymentService paymentService) : IRequestHandler<CreateOrderCommand, ServiceResult>
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

        var paymentRequest = new CreatePaymentRequest(order.OrderCode, request.PaymentInfo.CardNumber,
            request.PaymentInfo.CardHolderName, request.PaymentInfo.Expiration, request.PaymentInfo.Cvc,
            order.TotalPrice);
        var paymentResponse = await paymentService.CreatePaymentAsync(paymentRequest, cancellationToken);
        if (!paymentResponse.Status)
            return ServiceResult.Error(paymentResponse.ErrorMessage!, HttpStatusCode.InternalServerError);

        order.SetStatusToPaid(paymentResponse.PaymentId!.Value);

        orderRepository.Update(order);
        await unitOfWork.CommitAsync(cancellationToken);

        await publishEndpoint.Publish(new OrderCreatedEvent(order.Id, identityService.UserId), cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}