using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.UseCases.Identity;
using RentalPlatform.Domain.Enums;

namespace RentalPlatform.Web.Pages.Front;

public class RegisterModel : PageModel
{
    private readonly RegisterUserUseCase _registerUser;

    public RegisterModel(RegisterUserUseCase registerUser)
    {
        _registerUser = registerUser;
    }

    [BindProperty]
    public string FirstName { get; set; } = string.Empty;

    [BindProperty]
    public string LastName { get; set; } = string.Empty;

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    public string Role { get; set; } = "guest";

    public string? ErrorMessage { get; private set; }
    public string? SuccessMessage { get; private set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var userRole = Role.ToLowerInvariant() == "owner" ? UserRole.Owner : UserRole.Guest;

            var result = await _registerUser.ExecuteAsync(new RegisterUserCommand(
                FirstName,
                LastName,
                Email,
                Password,
                userRole
            ));

            SuccessMessage = "Registration successful! You can now sign in.";
            return Page();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return Page();
        }
    }
}