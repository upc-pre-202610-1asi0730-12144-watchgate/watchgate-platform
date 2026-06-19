using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Entities;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Aggregates;

public class Warehouse
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public int Capacity { get; private set; }
    public string Status { get; private set; } = "ACTIVE";
    public TimeSpan? OperationStart { get; private set; }
    public TimeSpan? OperationEnd { get; private set; }
    public int CompanyId { get; private set; }

    public ICollection<WarehouseZone> Zones { get; private set; } = new List<WarehouseZone>();

    protected Warehouse() { }

    public Warehouse(string name, string location, int capacity, int companyId,
        TimeSpan? operationStart = null, TimeSpan? operationEnd = null)
    {
        Name = name;
        Location = location;
        Capacity = capacity;
        CompanyId = companyId;
        OperationStart = operationStart;
        OperationEnd = operationEnd;
        Status = "ACTIVE";
    }

    public void Update(string name, string location, int capacity,
        TimeSpan? operationStart, TimeSpan? operationEnd)
    {
        Name = name;
        Location = location;
        Capacity = capacity;
        OperationStart = operationStart;
        OperationEnd = operationEnd;
    }

    public void Deactivate() => Status = "INACTIVE";
    public void Activate() => Status = "ACTIVE";
    public void EstablishHoursOfOperation(TimeSpan start, TimeSpan end) { OperationStart = start; OperationEnd = end; }
}
