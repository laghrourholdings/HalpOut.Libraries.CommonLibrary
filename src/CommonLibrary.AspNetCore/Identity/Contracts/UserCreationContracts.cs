﻿namespace CommonLibrary.AspNetCore.Identity;

//public record UserCreated(IServiceBusPayload<User> Payload);
public record UserCreated(UserBadge UserBadge);
public record InvalidateUser(Guid UserId);

//public record CreateUser(UserCredentialsDto CredentialsDto);