using CommonLibrary.Identity.Models.Dtos;

namespace CommonLibrary.AspNetCore.Identity;

//public record UserCreated(IServiceBusPayload<User> Payload);
public record UserCreated(Guid UserId, Guid LogHandleId);
public record CreateUser(UserCredentialsDto CredentialsDto);