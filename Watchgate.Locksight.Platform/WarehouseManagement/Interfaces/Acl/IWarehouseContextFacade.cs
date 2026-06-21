namespace Watchgate.Locksight.Platform.WarehouseManagement.Interfaces.Acl;

/// <summary>
/// Anti-Corruption Layer exposed by the WarehouseManagement bounded context. Other bounded contexts
/// must depend on this facade instead of reaching into WarehouseManagement's repositories, commands
/// or queries directly.
/// </summary>
public interface IWarehouseContextFacade
{
    Task<IEnumerable<int>> FetchZoneIdsByWarehouseIdAsync(int warehouseId, CancellationToken cancellationToken = default);
}
