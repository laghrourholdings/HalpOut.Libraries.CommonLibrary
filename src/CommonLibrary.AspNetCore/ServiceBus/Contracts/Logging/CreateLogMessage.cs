using CommonLibrary.Logging.Models;

namespace CommonLibrary.AspNetCore.ServiceBus.Contracts.Logging;

//public record CreateLogMessage(IServiceBusPayload<CommonLibrary.Logging.LogMessage> Payload);

//public record CreateLogMessages(IServiceBusPayloads<CommonLibrary.Logging.LogMessage> Payload);
public record CreateLogMessage(LogMessage LogMessage);
public record CreateLogMessages(IEnumerable<LogMessage> LogMessages);
