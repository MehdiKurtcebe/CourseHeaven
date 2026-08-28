using CourseHeaven.Web.PageModels;
using CourseHeaven.Web.Services;
using CourseHeaven.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.Web.Pages;

public class IndexModel(CatalogService catalogService, ILogger<IndexModel> logger) : BasePageModel
{
    public List<CourseViewModel>? Courses { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        var coursesAsResult = await catalogService.GetAllCoursesAsync();
        if (coursesAsResult.IsFail) return ErrorPage(coursesAsResult);

        Courses = coursesAsResult.Data!;

        return Page();
    }
}