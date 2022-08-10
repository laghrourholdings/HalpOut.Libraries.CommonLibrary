using CommonLibrary.Entities.InternalService;
using CommonLibrary.Implementations;
using CommonLibrary.Interfaces;

namespace CommonLibrary.Contracts.Gateway_Internal_Contracts;

//Gateway to Internal
public record RegisterObject(IObjectServiceBusRequest<IObject> Payload);
public record CreateObject(IServiceBusRequest<Guid> Payload);
public record GetAllObjects(IServiceBusRequest<IEmpty> Payload);


//Internal to Gateway
public record CreateObjectResponse(
    IObjectServiceBusResponse<Guid,IObject> Payload);
public record RegisterObjectResponse(IServiceBusResponse<IObject, Guid> Payload);
public record GetAllObjectResponse(IServiceBusResponse<IEmpty,IEnumerable<IObject>> Payload);
// public record CatalogItemUpdated(Guid ItemId, string Name, string Description);
// public record CatalogItemDeleted(Guid ItemId);