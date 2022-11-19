using CommonLibrary.AspNetCore.ServiceBus;
using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.Contracts.Objects;

//Gateway to Internal
public record CreateObject(IServiceBusMessage Payload);
//Internal to Log
//public record LogCreateObject(ServiceBusLogContext<ServiceBusPayload<IIObject>> Payload);
//Log to Internal
public record UpdateObjectLogHandle(IServiceBusPayload<IIObject> Payload);
//Internal to Log
public record ObjectCreated(IServiceBusMessageResponse<IIObject> Payload);