using System.Text.Json;
using CourseHeaven.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHeaven.Web.PageModels;

public class BasePageModel : PageModel
{
    public IActionResult ErrorPage(ServiceResult serviceResult, string? redirectUrl = null)
    {
        TempData["Error_Title"] = serviceResult.Fail!.Title;
        TempData["Error_Detail"] = serviceResult.Fail!.Detail;
        
        if (redirectUrl is not null)
            return RedirectToPage(redirectUrl);
        
        var validationError = serviceResult.Fail.Extensions.FirstOrDefault(x => x.Key == "errors");
        if (validationError.Value is null) return Page();
        
        var validationErrorAsDictionary = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(validationError.Value.ToString()!);
        foreach (var fieldError in validationErrorAsDictionary!.SelectMany(fieldErrors => fieldErrors.Value))
            ModelState.AddModelError(string.Empty, fieldError);
        
        return Page();
    }

    public IActionResult SuccessPage(string message, string? redirectUrl = null)
    {
        TempData["Success"] = true;
        TempData["Success_Message"] = message;
        
        if (redirectUrl is not null)
            return RedirectToPage(redirectUrl);
        
        return Page();
    }
}