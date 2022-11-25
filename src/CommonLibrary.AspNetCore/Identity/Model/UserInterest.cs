﻿using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.Identity.Model;

public class UserInterest : IObject
{
    public Guid Id { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public string? Descriptor { get; set; }
}