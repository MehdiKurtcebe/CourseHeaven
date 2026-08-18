using CourseHeaven.Web.Pages.Instructor.Dto;
using Refit;

namespace CourseHeaven.Web.Services.Refit;

public interface ICatalogRefitService
{
    [Post("/v1/catalog/courses")]
    Task<ApiResponse<ServiceResult>> CreateCourseAsync(CreateCourseRequest request);

    [Put("/v1/catalog/courses")]
    Task<ApiResponse<ServiceResult>> UpdateCourseAsync(UpdateCourseRequest request);

    [Delete("/v1/catalog/courses/{courseId}")]
    Task<ApiResponse<ServiceResult>> DeleteCourseAsync(Guid courseId);
}