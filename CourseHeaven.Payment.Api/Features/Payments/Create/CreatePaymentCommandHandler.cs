using System.Net;
using CourseHeaven.Payment.Api.Repositories;
using CourseHeaven.Shared;
using CourseHeaven.Shared.Services;
using MediatR;

namespace CourseHeaven.Payment.Api.Features.Payments.Create;

public class CreatePaymentCommandHandler(
    AppDbContext context,
    IIdentityService identityService)
    : IRequestHandler<CreatePaymentCommand, ServiceResult<CreatePaymentResponse>>
{
    public async Task<ServiceResult<CreatePaymentResponse>> Handle(CreatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var (isSuccess, errorMessage) = await ExternalPaymentProcessAsync(request.CardNumber, request.CardHolderName,
            request.CardExpirationDate, request.CardSecurityCode, request.Amount);

        if (!isSuccess)
            return ServiceResult<CreatePaymentResponse>.Error("Payment failed", errorMessage!,
                HttpStatusCode.BadRequest);

        var userId = identityService.UserId;
        var newPayment = new Repositories.Payment(userId, request.OrderCode, request.Amount)
        {
            Status = PaymentStatus.Success
        };

        context.Payments.Add(newPayment);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult<CreatePaymentResponse>.SuccessAsOk(new CreatePaymentResponse(newPayment.Id, true, null));
    }

    private async Task<(bool isSuccess, string? errorMessage)> ExternalPaymentProcessAsync(string cardNumber,
        string cardHolderName, string cardExpirationDate, string cardSecurityCode, decimal amount)
    {
        // Simulate external payment processing
        await Task.Delay(1000);

        // return (false, "Payment failed");
        return (true, null);
    }
}