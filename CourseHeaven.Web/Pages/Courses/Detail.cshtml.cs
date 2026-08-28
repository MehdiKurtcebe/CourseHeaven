using CourseHeaven.Web.PageModels;
using CourseHeaven.Web.Services;
using CourseHeaven.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.Web.Pages.Courses;

[AllowAnonymous]
public class DetailModel(CatalogService catalogService) : BasePageModel
{
    public CourseViewModel? Course { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var courseAsResult = await catalogService.GetCourseAsync(id);
        if (courseAsResult.IsFail) return ErrorPage(courseAsResult);

        Course = courseAsResult.Data!;
        return Page();
    }
}