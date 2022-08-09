using CommonLibrary.Entities.InternalService;

namespace CommonLibrary.Contracts.Gateway_Internal_Contracts;

public record CreateObject(IObject obj);
public record ObjectCreated(IObject obj);
// public record CatalogItemUpdated(Guid ItemId, string Name, string Description);
// public record CatalogItemDeleted(Guid ItemId);