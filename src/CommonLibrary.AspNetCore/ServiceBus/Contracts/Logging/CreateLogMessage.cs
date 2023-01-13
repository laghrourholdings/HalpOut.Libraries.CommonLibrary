using CommonLibrary.Logging.Models.Dtos;

namespace CommonLibrary.AspNetCore.ServiceBus.Contracts.Logging;

//public record CreateLogMessage(IServiceBusPayload<CommonLibrary.Logging.LogMessage> Payload);

//public record CreateLogMessages(IServiceBusPayloads<CommonLibrary.Logging.LogMessage> Payload);
public record CreateLogMessage(LogMessageDto LogMessage, Guid LogHandleId);