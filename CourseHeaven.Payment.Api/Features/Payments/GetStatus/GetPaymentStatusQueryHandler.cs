using CourseHeaven.Payment.Api.Repositories;
using CourseHeaven.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseHeaven.Payment.Api.Features.Payments.GetStatus;

public class GetPaymentStatusQueryHandler(AppDbContext context)
    : IRequestHandler<GetPaymentStatusQuery, ServiceResult<GetPaymentStatusResponse>>
{
    public async Task<ServiceResult<GetPaymentStatusResponse>> Handle(GetPaymentStatusQuery request,
        CancellationToken cancellationToken)
    {
        var payment =
            await context.Payments.FirstOrDefaultAsync(p => p.OrderCode == request.OrderCode, cancellationToken);

        if (payment is null)
            return ServiceResult<GetPaymentStatusResponse>.SuccessAsOk(new GetPaymentStatusResponse(null, false));

        return ServiceResult<GetPaymentStatusResponse>.SuccessAsOk(
            new GetPaymentStatusResponse(payment.Id, payment.Status == PaymentStatus.Success));
    }
}