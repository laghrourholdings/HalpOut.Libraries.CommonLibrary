namespace CommonLibrary.ClientServices.Identity.AuthService;

public interface IAuthService
{
    Task Login(/*LoginRequest loginRequest*/);
    Task Register(/*RegisterRequest registerRequest*/);
    Task Logout();
}