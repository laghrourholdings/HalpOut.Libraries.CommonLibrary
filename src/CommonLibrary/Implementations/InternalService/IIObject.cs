using CommonLibrary.Entities.InternalService;

namespace CommonLibrary.Implementations.InternalService;

public class IIObject: IObject
{
    public Guid Id { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedDate { get; set; }
    public bool IsSuspended { get; set; }
    public DateTimeOffset SuspendedDate { get; set; }
    public Guid LogHandleId { get; set; }
    public string? Descriptor { get; set; }
}