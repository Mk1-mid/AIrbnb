using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.UseCases.Kyc;
using RentalPlatform.Domain.Enums;

namespace RentalPlatform.Web.Pages.Front;

public class IdentityVerificationModel : PageModel
{
    private readonly ProcessKycUseCase _processKyc;

    public IdentityVerificationModel(ProcessKycUseCase processKyc)
    {
        _processKyc = processKyc;
    }

    [BindProperty]
    public IFormFile? Document { get; set; }

    [BindProperty]
    public Guid UserId { get; set; }

    public KycStatus? Status { get; private set; }
    public string Message { get; private set; } = "Please upload a valid government-issued ID to complete your profile verification.";

    public async Task OnPostAsync()
    {
        if (Document is null || Document.Length == 0)
        {
            ModelState.AddModelError(string.Empty, "Please choose an image file.");
            Status = KycStatus.Pending;
            return;
        }

        if (UserId == Guid.Empty)
        {
            ModelState.AddModelError(string.Empty, "UserId is required for verification.");
            Status = KycStatus.Pending;
            return;
        }

        await using var stream = Document.OpenReadStream();
        var result = await _processKyc.ExecuteAsync(new ProcessKycCommand(UserId, stream, Document.FileName));

        Status = result.Status;
        Message = result.Message;
    }
}
