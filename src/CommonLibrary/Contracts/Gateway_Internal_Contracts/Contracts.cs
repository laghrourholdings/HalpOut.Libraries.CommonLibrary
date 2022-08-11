using CommonLibrary.Entities.InternalService;
using CommonLibrary.Implementations;
using CommonLibrary.Interfaces;

namespace CommonLibrary.Contracts.Gateway_Internal_Contracts;

//Gateway to Internal
public record RegisterObject(IObjectServiceBusRequest<IObject> Payload);
public record CreateObject(IServiceBusRequest<Guid> Payload);
public record GetAllObjects(IServiceBusRequest<IEmpty> Payload);


//Internal to Gateway
public record CreateObjectResponse(IObjectServiceBusResponse<IObject> Payload);
public record RegisterObjectResponse(IServiceBusResponse<Guid> Payload);
public record GetAllObjectsResponse(IObjectServiceBusResponse<IObject> Payload);
