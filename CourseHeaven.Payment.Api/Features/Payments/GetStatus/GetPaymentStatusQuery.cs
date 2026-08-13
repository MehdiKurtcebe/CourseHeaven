using CourseHeaven.Shared;

namespace CourseHeaven.Payment.Api.Features.Payments.GetStatus;

public record GetPaymentStatusQuery(string OrderCode) : IRequestByServiceResult<GetPaymentStatusResponse>;