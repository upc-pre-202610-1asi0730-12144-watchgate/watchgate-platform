using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Entities;

public class WarehouseZone
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public double Area { get; private set; }
    public string RiskLevel { get; private set; } = "LOW";
    public WarehouseId WarehouseId { get; private set; }

    protected WarehouseZone() { }

    public WarehouseZone(string name, double area, WarehouseId warehouseId, string riskLevel = "LOW")
    {
        Name = name;
        Area = area;
        WarehouseId = warehouseId;
        RiskLevel = riskLevel;
    }

    public void AssignRiskLevel(string riskLevel) => RiskLevel = riskLevel;
}
