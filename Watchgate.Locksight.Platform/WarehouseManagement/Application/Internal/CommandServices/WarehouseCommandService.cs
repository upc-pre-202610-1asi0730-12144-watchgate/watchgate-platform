using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Watchgate.Locksight.Platform.Resources.Errors;
using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.WarehouseManagement.Application.CommandServices;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Commands;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Entities;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Repositories;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Application.Internal.CommandServices;

public class WarehouseCommandService(
    IWarehouseRepository warehouseRepository,
    IWarehouseZoneRepository zoneRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer) : IWarehouseCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

    public async Task<Result<Warehouse>> Handle(CreateWarehouseCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var start = TryParseTimeSpan(command.OperationStart);
            var end = TryParseTimeSpan(command.OperationEnd);
            var warehouse = new Warehouse(command.Name, command.Location, command.Capacity, command.CompanyId, start, end);
            await warehouseRepository.AddAsync(warehouse, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Warehouse>.Success(warehouse);
        }
        catch (DbUpdateException)
        {
            return Result<Warehouse>.Failure(WarehouseManagementError.DatabaseError,
                _localizer[nameof(WarehouseManagementError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Warehouse>.Failure(WarehouseManagementError.InternalServerError,
                _localizer[nameof(WarehouseManagementError.InternalServerError)]);
        }
    }

    public async Task<Result<Warehouse>> Handle(UpdateWarehouseCommand command,
        CancellationToken cancellationToken = default)
    {
        var warehouse = await warehouseRepository.FindByIdAsync(command.WarehouseId, cancellationToken);
        if (warehouse is null)
            return Result<Warehouse>.Failure(WarehouseManagementError.WarehouseNotFound,
                _localizer[nameof(WarehouseManagementError.WarehouseNotFound)]);

        warehouse.Update(command.Name, command.Location, command.Capacity,
            TryParseTimeSpan(command.OperationStart), TryParseTimeSpan(command.OperationEnd));
        warehouseRepository.Update(warehouse);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Warehouse>.Success(warehouse);
    }

    public async Task<Result> Handle(DeleteWarehouseCommand command, CancellationToken cancellationToken = default)
    {
        var warehouse = await warehouseRepository.FindByIdAsync(command.WarehouseId, cancellationToken);
        if (warehouse is null)
            return Result.Failure(WarehouseManagementError.WarehouseNotFound,
                _localizer[nameof(WarehouseManagementError.WarehouseNotFound)]);
        warehouseRepository.Remove(warehouse);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<WarehouseZone>> Handle(CreateWarehouseZoneCommand command,
        CancellationToken cancellationToken = default)
    {
        var warehouse = await warehouseRepository.FindByIdAsync(command.WarehouseId, cancellationToken);
        if (warehouse is null)
            return Result<WarehouseZone>.Failure(WarehouseManagementError.WarehouseNotFound,
                _localizer[nameof(WarehouseManagementError.WarehouseNotFound)]);
        var zone = new WarehouseZone(command.Name, command.Area, command.WarehouseId, command.RiskLevel);
        await zoneRepository.AddAsync(zone, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<WarehouseZone>.Success(zone);
    }

    public async Task<Result<WarehouseZone>> Handle(UpdateZoneRiskLevelCommand command,
        CancellationToken cancellationToken = default)
    {
        var zone = await zoneRepository.FindByIdAsync(command.ZoneId, cancellationToken);
        if (zone is null)
            return Result<WarehouseZone>.Failure(WarehouseManagementError.ZoneNotFound,
                _localizer[nameof(WarehouseManagementError.ZoneNotFound)]);
        zone.AssignRiskLevel(command.RiskLevel);
        zoneRepository.Update(zone);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<WarehouseZone>.Success(zone);
    }

    private static TimeSpan? TryParseTimeSpan(string? value) =>
        value != null && TimeSpan.TryParse(value, out var result) ? result : null;
}
