using Refit;

namespace CourseHeaven.Order.Application.Contracts.Refit.PaymentService;

public interface IPaymentService
{
    [Post("/api/v1/payments")]
    Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest paymentRequest,
        CancellationToken cancellationToken);

    [Get("/api/v1/payments/status/{orderCode}")]
    Task<GetPaymentStatusResponse> GetStatusAsync(string orderCode);
}