using CourseHeaven.Web.Pages.Instructor.Dto;
using Refit;

namespace CourseHeaven.Web.Services.Refit;

public interface ICatalogRefitService
{
    [Get("/api/v1/categories")]
    Task<ApiResponse<List<CategoryDto>>> GetCategoriesAsync();

    [Multipart]
    [Post("/api/v1/courses")]
    Task<ApiResponse<object>> CreateCourseAsync(
        [AliasAs("Name")] string name,
        [AliasAs("Description")] string description,
        [AliasAs("Price")] decimal price,
        [AliasAs("Image")] StreamPart? image,
        [AliasAs("CategoryId")] string categoryId);

    [Put("/api/v1/courses")]
    Task<ApiResponse<object>> UpdateCourseAsync(UpdateCourseRequest request);

    [Delete("/api/v1/courses/{courseId}")]
    Task<ApiResponse<object>> DeleteCourseAsync(Guid courseId);
}