namespace CourseHeaven.Web.Pages.Order.ViewModel;

public record OrderItemViewModel(
    Guid ProductId,
    string ProductName,
    decimal ProductPrice);