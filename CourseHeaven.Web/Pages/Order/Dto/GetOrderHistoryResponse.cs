using CourseHeaven.Web.Pages.Order.ViewModel;

namespace CourseHeaven.Web.Pages.Order.Dto;

public record GetOrderHistoryResponse(DateTimeOffset CreatedAt, decimal TotalPrice, List<OrderItemViewModel> Items);