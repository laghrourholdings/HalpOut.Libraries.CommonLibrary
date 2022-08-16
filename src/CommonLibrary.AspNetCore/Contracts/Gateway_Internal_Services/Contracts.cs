using CommonLibrary.AspNetCore.ServiceBus;
using CommonLibrary.AspNetCore.ServiceBus.Interfaces;
using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.Contracts;

//Gateway to Internal
public record RegisterObject(IObjectServiceBusRequest<IIObject> Payload);
public record GetAllObjects(IServiceBusRequest<IIObject> Payload);


//Internal to Gateway
public record RegisterObjectResponse(IServiceBusMessageResponse<Guid> Payload);
public record GetAllObjectsResponse(IObjectServiceBusMessageResponse<IIObject> Payload);
