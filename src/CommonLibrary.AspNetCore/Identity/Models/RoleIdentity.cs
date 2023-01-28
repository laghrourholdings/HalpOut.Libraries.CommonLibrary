namespace CommonLibrary.AspNetCore.Identity;

public class RoleIdentity
{
    public string Name { get; set; }
    public List<RoleProperty> Properties { get; set; } = new();
}