namespace CommonLibrary.Core;

public interface IDeletable
{
    bool IsDeleted { get; set; }
    DateTimeOffset DeletedDate { get; set; }
    Guid DeletedBy { get; set; }
}