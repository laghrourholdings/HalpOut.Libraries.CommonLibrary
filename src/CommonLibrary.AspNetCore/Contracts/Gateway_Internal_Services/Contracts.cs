using CommonLibrary.AspNetCore.ServiceBus;
using CommonLibrary.AspNetCore.ServiceBus.Interfaces;
using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.Contracts;

//Gateway to Internal
public record RegisterObject(IObjectServiceBusRequest<IObject> Payload);
public record CreateObject(IServiceBusRequest<Guid> Payload);
public record GetAllObjects(IServiceBusRequest<IEmpty> Payload);


//Internal to Gateway
public record CreateObjectResponse(IServiceBusRequestResponse<Guid,IIObject> Payload);
public record RegisterObjectResponse(IServiceBusMessageResponse<Guid> Payload);
public record GetAllObjectsResponse(IObjectServiceBusMessageResponse<IObject> Payload);
