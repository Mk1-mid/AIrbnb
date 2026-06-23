using RentalPlatform.Application.Interfaces;

namespace RentalPlatform.Application.UseCases.Dashboard;

public record GetOwnerDashboardQuery(
    Guid OwnerId,
    Guid? PropertyId,
    DateTime From,
    DateTime To
);

public class GetOwnerDashboardUseCase
{
    private readonly IDashboardRepository _dashboard;

    public GetOwnerDashboardUseCase(IDashboardRepository dashboard)
    {
        _dashboard = dashboard;
    }

    public Task<DashboardResult> ExecuteAsync(GetOwnerDashboardQuery query)
    {
        return _dashboard.GetOwnerMetricsAsync(
            query.OwnerId,
            query.PropertyId,
            query.From,
            query.To);
    }
}
