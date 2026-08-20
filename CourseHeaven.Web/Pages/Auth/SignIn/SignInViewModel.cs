using System.ComponentModel.DataAnnotations;

namespace CourseHeaven.Web.Pages.Auth.SignIn;

public record SignInViewModel
{
    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public required string Email { get; init; }

    [Display(Name = "Password")]
    [Required(ErrorMessage = "Password is required.")]
    public required string Password { get; init; }

    public static SignInViewModel Empty => new()
    {
        Email = string.Empty,
        Password = string.Empty
    };

    public static SignInViewModel GetSampleModel => new()
    {
        Email = "ali@veli.com",
        Password = "pass1234"
    };
}