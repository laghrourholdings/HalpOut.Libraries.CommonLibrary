namespace CommonLibrary.Core;

public interface IObject
{
    Guid Id { get; set; }
    DateTimeOffset CreationDate { get; set; }
    bool IsDeleted { get; set; }
    DateTimeOffset DeletedDate { get; set; }
    bool IsSuspended { get; set; }
    DateTimeOffset SuspendedDate { get; set; }
    Guid LogHandleId { get; set; }
    string? Descriptor { get; set; }
}