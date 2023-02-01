namespace CommonLibrary.ClientServices.Identity;

public interface ISecuroman
{
    public bool TrySignInWithUsername(string username, string password, out User user);
    public bool TrySignInWithEmail(string email, string password, out User user);
    public bool SignOut();

    public bool IsSignedIn(Guid userId);
    
}

public class Securoman : ISecuroman
{
    public bool TrySignInWithUsername(string username, string password, out User user)
    {
        throw new NotImplementedException();
    }
    
    public bool TrySignInWithEmail(string email, string password, out User user)
    {
        throw new NotImplementedException();
    }

    public bool SignOut()
    {
        throw new NotImplementedException();
    }

    public bool IsSignedIn(Guid userId)
    {
        throw new NotImplementedException();
    }
}