using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.WarehouseManagement.Application.QueryServices;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Entities;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Queries;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Repositories;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Application.Internal.QueryServices;

public class WarehouseQueryService(
    IWarehouseRepository warehouseRepository,
    IWarehouseZoneRepository zoneRepository) : IWarehouseQueryService
{
    public async Task<Result<IEnumerable<Warehouse>>> Handle(GetAllWarehousesQuery query,
        CancellationToken cancellationToken = default)
    {
        var warehouses = await warehouseRepository.ListAsync(cancellationToken);
        return Result<IEnumerable<Warehouse>>.Success(warehouses);
    }

    public async Task<Result<Warehouse>> Handle(GetWarehouseByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var warehouse = await warehouseRepository.FindByIdWithZonesAsync(query.WarehouseId, cancellationToken);
        if (warehouse is null)
            return Result<Warehouse>.Failure(WarehouseManagementError.WarehouseNotFound,
                $"Warehouse with id {query.WarehouseId} was not found.");
        return Result<Warehouse>.Success(warehouse);
    }

    public async Task<Result<IEnumerable<Warehouse>>> Handle(GetWarehousesByCompanyIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var warehouses = await warehouseRepository.FindByCompanyIdAsync(query.CompanyId, cancellationToken);
        return Result<IEnumerable<Warehouse>>.Success(warehouses);
    }

    public async Task<Result<IEnumerable<WarehouseZone>>> Handle(GetAllZonesByWarehouseIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var zones = await zoneRepository.FindByWarehouseIdAsync(query.WarehouseId, cancellationToken);
        return Result<IEnumerable<WarehouseZone>>.Success(zones);
    }
}
