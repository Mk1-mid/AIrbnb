using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.UseCases.Kyc;
using RentalPlatform.Domain.Enums;

namespace RentalPlatform.Web.Pages.Front;

[Authorize]
public class IdentityVerificationModel : PageModel
{
    private readonly ProcessKycUseCase _processKyc;

    public IdentityVerificationModel(ProcessKycUseCase processKyc)
    {
        _processKyc = processKyc;
    }

    [BindProperty]
    public IFormFile? Document { get; set; }

    public KycStatus? Status { get; private set; }
    public string Message { get; private set; } = "Please upload a valid government-issued ID to complete your profile verification.";

    public async Task OnGetAsync()
    {
        // Default message - KYC status pending
    }

    public async Task OnPostAsync()
    {
        if (Document is null || Document.Length == 0)
        {
            ModelState.AddModelError(string.Empty, "Please choose an image file.");
            Status = KycStatus.Pending;
            return;
        }

        var userIdClaim = User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            ModelState.AddModelError(string.Empty, "User not authenticated.");
            Status = KycStatus.Pending;
            return;
        }

        await using var stream = Document.OpenReadStream();
        var result = await _processKyc.ExecuteAsync(new ProcessKycCommand(userId, stream, Document.FileName));

        Status = result.Status;
        Message = result.Message;
    }
}
