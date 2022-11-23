using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.Identity.ModelDtos;

public class UserCredentials : IObject
{
    public Guid Id { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public string? Descriptor { get; set; }
    public string Email;
    public string Username;
    public string PhoneNumber;
    public string Password;
    
}