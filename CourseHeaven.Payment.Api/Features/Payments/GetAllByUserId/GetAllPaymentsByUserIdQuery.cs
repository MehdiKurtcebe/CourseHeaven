using CourseHeaven.Shared;

namespace CourseHeaven.Payment.Api.Features.Payments.GetAllByUserId;

public record GetAllPaymentsByUserIdQuery : IRequestByServiceResult<List<GetAllPaymentsByUserIdResponse>>;