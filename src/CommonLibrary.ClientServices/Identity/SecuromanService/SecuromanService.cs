using CommonLibrary.ClientServices.Identity.Handler;
using CommonLibrary.Identity;
using CommonLibrary.Identity.Dtos;
using Flurl.Http;

namespace CommonLibrary.ClientServices.Identity;

public interface ISecuroman
{
    public Task<User?> SignInWithUsername(string username, string password);
    public Task<User?> SignInWithEmail(string email, string password);
    public void SignOut();
    public bool IsSignedIn(Guid userId);
    
}

public class Securoman : ISecuroman
{
    private readonly IFlurlClient _httpClient;
    private readonly ICookie _cookie;

    public Securoman(HttpClient httpClient, ICookie cookie)
    {
        _httpClient = new FlurlClient(httpClient);
        _cookie = cookie;
    }
    
    public async Task<User?> SignInWithUsername(string username, string password)
    {
        var postData = new UserCredentialsDto
        {
            Username = username,
            Password = password,
            Email = ""
        };
        try
        {
            var loginResult = await  _httpClient.Request("user/login")
                .PostJsonAsync(postData);
            var tokenCookie =  await _cookie.GetValue(SecuromanDefaults.TokenCookie);
            Console.WriteLine($"tokenCookie: ${tokenCookie}");
            if(string.IsNullOrEmpty(tokenCookie))
                return null;
            var tokenPayload = SecuromanTokenizer.GetUserClaims(tokenCookie);
            if(tokenPayload == null)
                return null;
            Console.WriteLine(tokenPayload);
            var user = new User
            {
                Id = new Guid(tokenPayload.First(x=>x.Type == UserClaimTypes.Id).Value),
                LogHandleId = new Guid(tokenPayload.First(x=>x.Type == UserClaimTypes.LogHandleId).Value),
                UserClaims = tokenPayload.Where(x=>x.Type != UserClaimTypes.Id && x.Type != UserClaimTypes.LogHandleId).ToList()
            };
            return user;
        }
        catch (FlurlHttpException exception)
        {
            Console.WriteLine(exception.Message);
        }
        return null;
    }
    
    public Task<User?> SignInWithEmail(string email, string password)
    {
        throw new NotImplementedException();
    }

    public async void SignOut()
    {
        try
        {
            var loginResult = await _httpClient.Request("user/signout").GetAsync();
        }
        catch
        {
            
        }
    }

    public bool IsSignedIn(Guid userId)
    {
        throw new NotImplementedException();
    }
}