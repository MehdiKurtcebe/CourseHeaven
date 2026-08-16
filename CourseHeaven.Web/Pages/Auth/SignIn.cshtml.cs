using CourseHeaven.Web.Pages.Auth.SignIn;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHeaven.Web.Pages.Auth;

public class SignInModel(SignInService signInService) : PageModel
{
    [BindProperty] public required SignInViewModel SignInViewModel { get; set; } = SignInViewModel.Empty;
    
    public void OnGet()
    {
    }
    
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return Page();

        var result = await signInService.AuthenticateAsync(SignInViewModel, cancellationToken);
        if (result.IsFail)
        {
            ModelState.AddModelError(string.Empty, result.Fail!.Title!);
            ModelState.AddModelError(string.Empty, result.Fail.Detail!);
            return Page();
        }

        return RedirectToPage("/Index");
    }
}