namespace CourseHeaven.Web.Pages.Instructor.ViewModel;

public record CourseViewModel(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string? ImageUrl,
    string CategoryName,
    int Duration,
    double Rating)
{
    public string TruncateDescription(int maxLength)
    {
        return Description.Length <= maxLength ? Description : string.Concat(Description.AsSpan(0, maxLength), "...");
    }
}