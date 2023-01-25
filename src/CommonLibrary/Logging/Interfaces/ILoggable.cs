namespace CommonLibrary.Logging;

/// <summary>
/// Interface that asserts that a class can be used to log messages to the LogService through its logHandleId
/// </summary>
public interface IBusinessObject
{
    Guid LogHandleId { get; set; }
}