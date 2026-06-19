using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Commands;
using Watchgate.Locksight.Platform.WarehouseManagement.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Interfaces.Rest.Transform;

public static class CreateWarehouseCommandFromResourceAssembler
{
    public static CreateWarehouseCommand ToCommandFromResource(CreateWarehouseResource resource) =>
        new(resource.Name, resource.Location, resource.Capacity, resource.CompanyId,
            resource.OperationStart, resource.OperationEnd);
}
