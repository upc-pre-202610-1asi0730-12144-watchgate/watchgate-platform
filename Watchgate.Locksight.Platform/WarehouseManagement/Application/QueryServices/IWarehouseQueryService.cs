using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.WarehouseManagement.Application.Model;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Entities;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Queries;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Application.QueryServices;

public interface IWarehouseQueryService
{
    Task<Result<IEnumerable<Warehouse>>> Handle(GetAllWarehousesQuery query, CancellationToken cancellationToken = default);
    Task<Result<Warehouse>> Handle(GetWarehouseByIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Warehouse>>> Handle(GetWarehousesByCompanyIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<WarehouseZone>>> Handle(GetAllZonesByWarehouseIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<WarehouseDashboard>> Handle(GetWarehouseDashboardByCompanyIdQuery query, CancellationToken cancellationToken = default);
}
