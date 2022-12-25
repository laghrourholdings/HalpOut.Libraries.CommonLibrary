namespace CommonLibrary.AspNetCore.Contracts.Logging;

//public record CreateLogMessage(IServiceBusPayload<CommonLibrary.Logging.LogMessage> Payload);

//public record CreateLogMessages(IServiceBusPayloads<CommonLibrary.Logging.LogMessage> Payload);
public record CreateLogMessage(CommonLibrary.Logging.LogMessage LogMessage);
public record CreateLogMessages(IEnumerable<CommonLibrary.Logging.LogMessage> LogMessages);
