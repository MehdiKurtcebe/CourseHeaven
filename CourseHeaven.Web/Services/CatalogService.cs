using CourseHeaven.Web.Extensions;
using CourseHeaven.Web.Services.Refit;
using CourseHeaven.Web.ViewModel;
using Refit;

namespace CourseHeaven.Web.Services;

public class CatalogService(
    ICatalogRefitService catalogRefitService,
    UserService userService,
    ILogger<CatalogService> logger)
{
    public async Task<ServiceResult<List<CategoryViewModel>>> GetCategoriesAsync()
    {
        var response = await catalogRefitService.GetCategoriesAsync();
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Error occurred while fetching categories");
            return ServiceResult<List<CategoryViewModel>>.Error("Fail to retrieve categories. Please try again later");
        }

        var categories = response.Content!
            .Select(c => new CategoryViewModel(c.Id, c.Name))
            .ToList();
        return ServiceResult<List<CategoryViewModel>>.Success(categories);
    }

    public async Task<ServiceResult> CreateCourseAsync(CreateCourseViewModel viewModel)
    {
        StreamPart? pictureStreamPart = null;
        await using var stream = viewModel.ImageFormFile?.OpenReadStream();

        if (viewModel.ImageFormFile is not null && viewModel.ImageFormFile.Length > 0)
            pictureStreamPart = new StreamPart(stream!, viewModel.ImageFormFile.FileName,
                viewModel.ImageFormFile.ContentType);

        var response = await catalogRefitService.CreateCourseAsync(
            viewModel.Name,
            viewModel.Description,
            viewModel.Price,
            pictureStreamPart,
            viewModel.CategoryId.ToString()!
        );

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Error occurred while creating course");
            return ServiceResult.Error("Fail to create course. Please try again later");
        }

        return ServiceResult.Success();
    }

    public async Task<ServiceResult<List<CourseViewModel>>> GetAllCoursesAsync()
    {
        var coursesAsResult = await catalogRefitService.GetAllCoursesAsync();
        if (!coursesAsResult.IsSuccessStatusCode)
        {
            logger.LogProblemDetailsExtension(coursesAsResult.Error as ApiException);
            return ServiceResult<List<CourseViewModel>>.Error(
                "Failed to retrieve course data. Please try again later.");
        }

        var courses = coursesAsResult.Content!;
        var coursesViewModel = courses.Select(c =>
            new CourseViewModel(c.Id, c.Name, c.Description, c.Price, c.ImageUrl,
                c.CreatedAt.LocalDateTime.ToLongDateString(),
                c.Feature.EducatorFullName, c.Category.Name, c.Feature.Duration, c.Feature.Rating)).ToList();

        return ServiceResult<List<CourseViewModel>>.Success(coursesViewModel);
    }

    public async Task<ServiceResult<CourseViewModel>> GetCourseAsync(Guid courseId)
    {
        var response = await catalogRefitService.GetCourseAsync(courseId);
        if (!response.IsSuccessStatusCode)
            return ServiceResult<CourseViewModel>.FailFromProblemDetails((response.Error as ApiException)!);

        var course = response.Content!;
        var courseViewModel = new CourseViewModel(course.Id, course.Name, course.Description, course.Price,
            course.ImageUrl, course.CreatedAt.LocalDateTime.ToLongDateString(), course.Feature.EducatorFullName,
            course.Category.Name,
            course.Feature.Duration, course.Feature.Rating);

        return ServiceResult<CourseViewModel>.Success(courseViewModel);
    }

    public async Task<ServiceResult<List<CourseViewModel>>> GetCoursesByUserIdAsync()
    {
        var course = await catalogRefitService.GetCoursesByUserIdAsync(userService.UserId);

        if (!course.IsSuccessStatusCode)
        {
            logger.LogError("Error occurred while fetching courses by user id");
            return ServiceResult<List<CourseViewModel>>.Error("Fail to retrieve courses. Please try again later");
        }

        var courses = course.Content!
            .Select(c => new CourseViewModel(
                c.Id,
                c.Name,
                c.Description,
                c.Price,
                c.ImageUrl,
                c.CreatedAt.LocalDateTime.ToLongDateString(),
                c.Feature.EducatorFullName,
                c.Category.Name,
                c.Feature.Duration,
                c.Feature.Rating
            ))
            .ToList();

        return ServiceResult<List<CourseViewModel>>.Success(courses);
    }

    public async Task<ServiceResult> DeleteAsync(Guid courseId)
    {
        var response = await catalogRefitService.DeleteCourseAsync(courseId);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Error occurred while deleting course");
            return ServiceResult.Error("Fail to delete course. Please try again later");
        }

        return ServiceResult.Success();
    }
}