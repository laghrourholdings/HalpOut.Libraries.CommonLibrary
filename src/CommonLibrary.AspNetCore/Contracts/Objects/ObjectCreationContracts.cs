using CommonLibrary.AspNetCore.ServiceBus;
using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.Contracts.Objects;

//Gateway to Internal
public record CreateObject();
public record UpdateObjectLogHandle(Guid ObjectId, Guid LogHandleId);
//Internal to Log
public record ObjectCreated(Guid ObjectId);