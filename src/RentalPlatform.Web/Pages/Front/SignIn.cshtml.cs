using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.UseCases.Identity;
using RentalPlatform.Domain.Enums;
using System.Security.Claims;

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

    [FromQuery]
    public string? ReturnUrl { get; set; }

    public string? ErrorMessage { get; private set; }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var result = await _loginUser.ExecuteAsync(new LoginUserCommand(Email, Password));
            
            Response.Cookies.Append("jwt", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddHours(8)
            });

            // Redirigir a returnUrl si existe, sino por defecto según rol
            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            if (result.Role == UserRole.Owner)
            {
                return RedirectToPage("/Front/OwnerDashboard");
            }
            else
            {
                return RedirectToPage("/Front/GuestReservations");
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return Page();
        }
    }
}
