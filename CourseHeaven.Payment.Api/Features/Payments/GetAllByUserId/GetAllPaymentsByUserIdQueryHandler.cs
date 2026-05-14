using CourseHeaven.Payment.Api.Repositories;
using CourseHeaven.Shared;
using CourseHeaven.Shared.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseHeaven.Payment.Api.Features.Payments.GetAllByUserId;

public class GetAllPaymentsByUserIdQueryHandler(AppDbContext context, IIdentityService identityService)
    : IRequestHandler<GetAllPaymentsByUserIdQuery, ServiceResult<List<GetAllPaymentsByUserIdResponse>>>
{
    public async Task<ServiceResult<List<GetAllPaymentsByUserIdResponse>>> Handle(GetAllPaymentsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = identityService.UserId;

        var payments = await context.Payments
            .Where(p => p.UserId == userId)
            .Select(p => new GetAllPaymentsByUserIdResponse(
                p.Id,
                p.OrderCode,
                p.Amount,
                p.CreatedAt,
                p.Status))
            .ToListAsync(cancellationToken);

        return ServiceResult<List<GetAllPaymentsByUserIdResponse>>.SuccessAsOk(payments);
    }
}