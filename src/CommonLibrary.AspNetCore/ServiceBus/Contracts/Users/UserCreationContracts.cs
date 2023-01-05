using CommonLibrary.Identity.Models.Dtos;

namespace CommonLibrary.AspNetCore.ServiceBus.Contracts.Users;

//public record UserCreated(IServiceBusPayload<User> Payload);
public record UpdateUserLogHandle(Guid UserId, Guid LogHandleId);
public record UserCreated(Guid UserId);
public record CreateUser(UserCredentialsDto CredentialsDto);