namespace Watchgate.Locksight.Platform.SensorIntegration.Interfaces.Acl;

/// <summary>
/// Anti-Corruption Layer exposed by the SensorIntegration bounded context. Other bounded contexts
/// must depend on this facade instead of reaching into SensorIntegration's repositories, commands or
/// queries directly.
/// </summary>
public interface ISensorContextFacade
{
    Task<IEnumerable<int>> FetchSensorIdsByWarehouseIdAsync(int warehouseId, CancellationToken cancellationToken = default);
}
