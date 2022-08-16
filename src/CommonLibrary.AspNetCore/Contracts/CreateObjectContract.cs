using CommonLibrary.AspNetCore.Logging;
using CommonLibrary.AspNetCore.ServiceBus;
using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.Contracts;

//Gateway to Internal
public record CreateObject(IServiceBusMessage Payload);
//Internal to Log
public record LogCreateObject(ServiceBusLogContext<ServiceBusRequest<IIObject>> Payload);
//Log to Internal
public record LogCreateObjectResponse(ServiceBusMessageReponse<IIObject> Payload);
//Internal to Gateway
public record CreateObjectResponse(IServiceBusRequest<IIObject> Payload);