using ClosedXML.Excel;
using RentalPlatform.Application.Interfaces;

namespace RentalPlatform.Application.UseCases.Reports;

public record ExportReportCommand(
    Guid OwnerId,
    Guid? PropertyId,
    DateTime From,
    DateTime To
);

public class ExportReportUseCase
{
    private readonly IReportRepository _reports;

    public ExportReportUseCase(IReportRepository reports)
    {
        _reports = reports;
    }

    public async Task<byte[]> ExecuteAsync(ExportReportCommand cmd)
    {
        var reservations = await _reports.GetReservationsForReportAsync(
            cmd.OwnerId, cmd.PropertyId, cmd.From, cmd.To);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Reservations");

        worksheet.Cell(1, 1).Value = "Check-in";
        worksheet.Cell(1, 2).Value = "Check-out";
        worksheet.Cell(1, 3).Value = "Property";
        worksheet.Cell(1, 4).Value = "City";
        worksheet.Cell(1, 5).Value = "Guest";
        worksheet.Cell(1, 6).Value = "Guest Document";
        worksheet.Cell(1, 7).Value = "Price Paid";
        worksheet.Cell(1, 8).Value = "Currency";

        var row = 2;
        foreach (var r in reservations.OrderBy(x => x.StayPeriod.CheckIn))
        {
            worksheet.Cell(row, 1).Value = r.StayPeriod.CheckIn.ToLocalTime();
            worksheet.Cell(row, 2).Value = r.StayPeriod.CheckOut.ToLocalTime();
            worksheet.Cell(row, 3).Value = r.Property.Title;
            worksheet.Cell(row, 4).Value = r.Property.City;
            worksheet.Cell(row, 5).Value = $"{r.Guest.FirstName} {r.Guest.LastName}";
            worksheet.Cell(row, 6).Value = r.Guest.KycRecord?.DocumentNumber ?? string.Empty;
            worksheet.Cell(row, 7).Value = r.TotalPrice.Amount;
            worksheet.Cell(row, 8).Value = r.TotalPrice.Currency;
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
