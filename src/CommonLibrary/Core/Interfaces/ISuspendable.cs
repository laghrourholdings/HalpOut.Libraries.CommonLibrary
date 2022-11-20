namespace CommonLibrary.Core;

public interface ISuspendable
{
    bool IsSuspended { get; set; }
    DateTimeOffset SuspendedDate { get; set; }
    Guid SuspendedBy { get; set; }
}