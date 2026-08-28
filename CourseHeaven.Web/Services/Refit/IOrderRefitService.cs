using CourseHeaven.Web.Pages.Order.Dto;
using Refit;

namespace CourseHeaven.Web.Services.Refit;

public interface IOrderRefitService
{
    [Post("/api/v1/orders")]
    Task<ApiResponse<object>> CreateOrderAsync(CreateOrderRequest request);

    [Get("/api/v1/orders")]
    Task<ApiResponse<List<GetOrderHistoryResponse>>> GetOrdersAsync();
}