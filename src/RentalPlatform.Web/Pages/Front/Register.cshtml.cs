using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.UseCases.Identity;
using RentalPlatform.Domain.Enums;

namespace RentalPlatform.Web.Pages.Front;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly RegisterUserUseCase _registerUserUseCase;

    public RegisterModel(RegisterUserUseCase registerUserUseCase)
    {
        _registerUserUseCase = registerUserUseCase;
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

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostRegisterAsync()
    {
        if (!Enum.TryParse<UserRole>(Role, true, out var parsedRole))
        {
            ModelState.AddModelError(string.Empty, "Rol inválido.");
            return Page();
        }

        try
        {
            await _registerUserUseCase.ExecuteAsync(new RegisterUserCommand(
                FirstName,
                LastName,
                Email,
                Password,
                parsedRole));

            return RedirectToPage("/Front/SignIn");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
