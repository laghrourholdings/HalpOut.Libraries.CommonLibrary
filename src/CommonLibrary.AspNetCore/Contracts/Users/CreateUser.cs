using CommonLibrary.AspNetCore.Identity.Model;
using CommonLibrary.AspNetCore.Identity.ModelDtos;
using CommonLibrary.AspNetCore.ServiceBus;
using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.Contracts.Users;

public record UserCreated(IServiceBusPayload<User> Payload);
public record UpdateUserLogHandle(IServiceBusPayload<User> Payload);
public record CreateUser(IServiceBusPayload<UserCredentials> Payload);