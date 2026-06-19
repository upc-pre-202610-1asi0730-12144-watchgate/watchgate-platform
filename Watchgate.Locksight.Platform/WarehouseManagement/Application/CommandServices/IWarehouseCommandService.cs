using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Commands;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Entities;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Application.CommandServices;

public interface IWarehouseCommandService
{
    Task<Result<Warehouse>> Handle(CreateWarehouseCommand command, CancellationToken cancellationToken = default);
    Task<Result<Warehouse>> Handle(UpdateWarehouseCommand command, CancellationToken cancellationToken = default);
    Task<Result> Handle(DeleteWarehouseCommand command, CancellationToken cancellationToken = default);
    Task<Result<WarehouseZone>> Handle(CreateWarehouseZoneCommand command, CancellationToken cancellationToken = default);
    Task<Result<WarehouseZone>> Handle(UpdateZoneRiskLevelCommand command, CancellationToken cancellationToken = default);
}