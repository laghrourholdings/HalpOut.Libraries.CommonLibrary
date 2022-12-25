using CommonLibrary.AspNetCore.Identity.Model;
using CommonLibrary.AspNetCore.Identity.ModelDtos;
using CommonLibrary.AspNetCore.ServiceBus;

namespace CommonLibrary.AspNetCore.Contracts.Users;

//public record UserCreated(IServiceBusPayload<User> Payload);
public record UpdateUserLogHandle(Guid UserId, Guid LogHandleId);
public record UserCreated(Guid UserId);
public record CreateUser(UserCredentials Credentials);