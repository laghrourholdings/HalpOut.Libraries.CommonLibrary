using CommonLibrary.AspNetCore.Identity.Model.Dtos;

namespace CommonLibrary.AspNetCore.ServiceBus.Contracts.Users;

//public record UserCreated(IServiceBusPayload<User> Payload);
public record UpdateUserLogHandle(Guid UserId, Guid LogHandleId);
public record UserCreated(Guid UserId);
public record CreateUser(UserCredentials Credentials);