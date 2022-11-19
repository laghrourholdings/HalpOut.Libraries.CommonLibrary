using CommonLibrary.AspNetCore.ServiceBus;
using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.Contracts.LogMessage;

public record CreateLogMessage(IServiceBusPayload<CommonLibrary.Logging.LogMessage> Payload);

public record CreateLogMessages(IServiceBusPayloads<CommonLibrary.Logging.LogMessage> Payload);
