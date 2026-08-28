namespace CourseHeaven.Web.Pages.Basket.ViewModel;

public record BasketItemViewModel(
    Guid CourseId,
    string CourseName,
    string? CourseImageUrl,
    decimal CoursePrice,
    decimal? DiscountedPrice);