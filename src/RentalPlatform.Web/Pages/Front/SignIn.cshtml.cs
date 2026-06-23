using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.UseCases.Identity;

namespace RentalPlatform.Web.Pages.Front;

public class SignInModel : PageModel
{
    private readonly LoginUserUseCase _loginUser;

    public SignInModel(LoginUserUseCase loginUser)
    {
        _loginUser = loginUser;
    }

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public LoginUserResult? Result { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task OnPostAsync()
    {
        try
        {
            Result = await _loginUser.ExecuteAsync(new LoginUserCommand(Email, Password));
            Response.Cookies.Append("jwt", Result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(8)
            });
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}
