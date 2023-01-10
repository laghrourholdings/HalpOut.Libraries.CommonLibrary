namespace CommonLibrary.Core;

public interface IObject
{
    DateTimeOffset CreationDate { get; set; }
    string? Descriptor { get; set; }
}