using CommonLibrary.AspNetCore.Logging;
using CommonLibrary.AspNetCore.ServiceBus;
using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.Contracts;

//Internal to Log

public record LogCreateObject(ServiceBusLogContext<ServiceBusRequest<IIObject>> Payload);


//Log to Internal
public record LogCreateObjectResponse(ServiceBusMessageReponse<IIObject> Payload);

