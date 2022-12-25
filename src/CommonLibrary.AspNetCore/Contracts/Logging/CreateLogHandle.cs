using CommonLibrary.AspNetCore.ServiceBus;

namespace CommonLibrary.AspNetCore.Contracts.Logging;

//public record CreateLogMessage(IServiceBusPayload<CommonLibrary.Logging.LogMessage> Payload);

//public record CreateLogMessages(IServiceBusPayloads<CommonLibrary.Logging.LogMessage> Payload);
public record CreateLogHandle(Guid ObjectId, string ObjectType);
//public record CreateLogHandle(Guid ObjectId, string ObjectType, IServiceBusPayload<st>);
