﻿using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.Identity.Model;

public class UserSession : IObject
{
    public Guid Id { get; set; }
    public Guid DeviceId;
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ExpirationDate;
    public string? Descriptor { get; set; }
}