using CommonLibrary.Entities.InternalService;
using CommonLibrary.Implementations.InternalService;

namespace CommonLibrary.Contracts.Gateway_Internal_Contracts;

//Gateway to Internal
public record CreateObject(IObject obj);
public record GetObject(IObject obj);

//Internal to Gateway
public record CreateObjectResponse(IObject obj);
public record GetObjectResponse(IObject obj);
// public record CatalogItemUpdated(Guid ItemId, string Name, string Description);
// public record CatalogItemDeleted(Guid ItemId);