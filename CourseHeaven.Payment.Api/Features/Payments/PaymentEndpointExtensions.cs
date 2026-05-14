using Asp.Versioning.Builder;
using CourseHeaven.Payment.Api.Features.Payments.Create;
using CourseHeaven.Payment.Api.Features.Payments.GetAllByUserId;

namespace CourseHeaven.Payment.Api.Features.Payments;

public static class PaymentEndpointExtensions
{
    public static void AddPaymentGroupEndpointExtension(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/payments")
            .WithTags("Payments")
            .WithApiVersionSet(apiVersionSet)
            .CreatePaymentGroupItemEndpoint()
            .GetAllPaymentsByUserIdGroupItemEndpoint();
    }
}