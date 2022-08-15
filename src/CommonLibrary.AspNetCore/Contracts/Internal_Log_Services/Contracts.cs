using CommonLibrary.AspNetCore.ServiceBus;

namespace CommonLibrary.AspNetCore.Contracts;




//Log to Internal
public record LogObjectCreateResponse(IServiceBusMessageResponse<Guid> Payload);