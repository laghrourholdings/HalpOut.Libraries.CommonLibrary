using System.ComponentModel.DataAnnotations;
using CommonLibrary.Logging;

namespace CommonLibrary.Core;

/// <summary>
/// The IIObject class is the base BOI required for all interservies operations and logic.
/// Inherits the IObject interface
/// </summary>
public class IIObject: IObject
{
    [Key]
    public Guid Id { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedDate { get; set; }
    public bool IsSuspended { get; set; }
    public DateTimeOffset SuspendedDate { get; set; }
    public Guid LogHandleId { get; set; }
    public LogHandle? LogHandle { get; set; }
    public string? Descriptor { get; set; }
}
