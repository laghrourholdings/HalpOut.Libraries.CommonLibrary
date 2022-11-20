namespace CommonLibrary.Core;

public interface IObject
{
    Guid Id { get; set; }
    DateTimeOffset CreationDate { get; set; }
    string? Descriptor { get; set; }
}