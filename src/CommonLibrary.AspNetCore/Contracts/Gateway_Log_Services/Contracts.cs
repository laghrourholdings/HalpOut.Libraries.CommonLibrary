using CommonLibrary.AspNetCore.Logging;
using CommonLibrary.AspNetCore.ServiceBus;
using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.Contracts;



//Gateway to Log

public record LogObjectCreate(IServiceBusLogContext<ServiceBusRequest<Guid>> Payload);

