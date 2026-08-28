using CourseHeaven.Web.Services;
using CourseHeaven.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHeaven.Web.Pages.Instructor;

public class CoursesModel(CatalogService catalogService) : PageModel
{
    public List<CourseViewModel> CourseViewModels { get; set; } = null!;

    public async Task OnGetAsync()
    {
        var result = await catalogService.GetCoursesByUserIdAsync();
        if (result.IsFail)
        {
            // TODO: redirect to error page
        }

        CourseViewModels = result.Data!;
    }

    public async Task<IActionResult> OnGetDeleteAsync(Guid courseId)
    {
        var result = await catalogService.DeleteAsync(courseId);
        if (result.IsFail)
        {
            //TODO: redirect to error page
        }

        return RedirectToPage();
    }
}