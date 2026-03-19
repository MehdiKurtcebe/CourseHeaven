namespace CourseHeaven.Basket.Api.Data;

public class BasketItem
{
    public BasketItem(Guid courseId, string courseName, decimal coursePrice, decimal? discountedPrice,
        string? courseImageUrl)
    {
        CourseId = courseId;
        CourseName = courseName;
        CoursePrice = coursePrice;
        DiscountedPrice = discountedPrice;
        CourseImageUrl = courseImageUrl;
    }

    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = null!;
    public decimal CoursePrice { get; set; }
    public decimal? DiscountedPrice { get; set; }
    public string? CourseImageUrl { get; set; }
}