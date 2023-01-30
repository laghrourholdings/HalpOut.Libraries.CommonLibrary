namespace CommonLibrary.Core;

public interface IUser :  IBusinessObject
{
    public Guid Id { get; set; }
}