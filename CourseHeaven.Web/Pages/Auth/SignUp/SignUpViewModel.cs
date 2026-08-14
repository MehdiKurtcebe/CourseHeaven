using System.ComponentModel.DataAnnotations;

namespace CourseHeaven.Web.Pages.Auth.SignUp;

public record SignUpViewModel
{
    [Display(Name = "First Name")]
    [Required(ErrorMessage = "First Name is required.")]
    public required string FirstName { get; init; }

    [Display(Name = "Last Name")]
    [Required(ErrorMessage = "Last Name is required.")]
    public required string LastName { get; init; }

    [Display(Name = "Username")]
    [Required(ErrorMessage = "Username is required.")]
    public required string Username { get; init; }

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public required string Email { get; init; }

    [Display(Name = "Password")]
    [Required(ErrorMessage = "Password is required.")]
    public required string Password { get; init; }

    [Display(Name = "Password Confirm")]
    [Required(ErrorMessage = "Password Confirm is required.")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public required string PasswordConfirm { get; init; }

    public static SignUpViewModel Empty =>
        new()
        {
            FirstName = string.Empty, LastName = string.Empty, Username = string.Empty, Email = string.Empty,
            Password = string.Empty, PasswordConfirm = string.Empty
        };

    public static SignUpViewModel GetSampleModel =>
        new()
        {
            FirstName = "Ali", LastName = "Veli", Username = "AliVeli", Email = "ali@veli.com", Password = "pass1234",
            PasswordConfirm = "pass1234"
        };
}