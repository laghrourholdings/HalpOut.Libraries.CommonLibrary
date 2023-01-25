
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using CommonLibrary.Identity.Models;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace CommonLibrary.AspNetCore.Identity;

public interface ISecuromanService
{
    Task<Securoman.AuthenticateResult> Authenticate(string token);
    //Task<TokenResult?> Authenticate();
    bool IsAuthenticated();
    Task SetOrUpdateUserAsync(UserBadge badge);
    Task RemoveUserAsync(Guid userId);
    Task<UserBadge?> GetUserFromSecuromanCache(Guid userId);
    public Guid? GetUnverifiedLogHandleId(string token);
}

public class SecuromanService : ISecuromanService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDistributedCache _cache;
    private readonly string _securomanUrl;

    public class UserBadgeCacheData
    {
        public Guid LogHandleId { get; set; }
        public byte[] SecretKey { get; set; }
        public RolePrincipal RolePrincipal { get; set; }
    }

    public SecuromanService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _cache = Securoman.GetSecuromanCache(config);
        _securomanUrl = Securoman.GetSecuromanUrl(config);
    }

    // public async Task<string?> TryTokenRefresh()
    // {
    //     var refreshRequestResult = await _securomanUrl
    //         .AppendPathSegment("refresh")
    //         .WithCookies(_httpContextAccessor.HttpContext?.Request.Cookies)
    //         .GetAsync();
    //     return 
    //         refreshRequestResult.StatusCode == StatusCodes.Status200OK 
    //             ? refreshRequestResult.Cookies?.FirstOrDefault(x=>x.Name == "Identity.Token")?.Value 
    //             : null;
    // }

    public Guid? GetUnverifiedLogHandleId(string token)
    {
        var payload = Securoman.GetUnverifiedUserTicket(token);
        var logHandleId = payload?.FirstOrDefault(x=>x.Type == UserClaimTypes.LogHandleId)?.Value;
        return logHandleId != null ? new Guid(logHandleId) : null;
    }
    
    public Guid? GetUnverifiedUserId(string token)
    {
        var payload = Securoman.GetUnverifiedUserTicket(token);
        var userId = payload?.FirstOrDefault(x=>x.Type == ClaimTypes.NameIdentifier)?.Value;
        return userId != null ? new Guid(userId) : null;
    }

    public bool IsAuthenticated()
    {
        if (_httpContextAccessor.HttpContext == null)
            return false;
        
        return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier) != null;
    }

    public async Task<Securoman.AuthenticateResult> Authenticate(string token)
    {
        var payload = Securoman.GetUnverifiedUserTicket(token);
        var userId = payload?.FirstOrDefault(x=>x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return  new Securoman.AuthenticateResult("UserId is null");;
        var userBadge = await GetUser(new Guid(userId));
        if (userBadge == null)
        {
            try
            {
                var userBadgerRequest = _securomanUrl
                    .WithCookies(_httpContextAccessor.HttpContext.Request.Cookies)
                    .AppendPathSegment("api/v1/auth")
                    .AppendPathSegment("refreshBadge");
                var updatedUserBadge = await userBadgerRequest.GetJsonAsync<UserBadge>();
                if (updatedUserBadge == null) return new Securoman.AuthenticateResult("Failed to grab user badge");
                await SetOrUpdateUserAsync(updatedUserBadge);
                userBadge = updatedUserBadge;
            }
            catch (FlurlHttpException ex)
            {
                return new Securoman.AuthenticateResult(ex.Message);
            }
        }
        var verify = Securoman.VerifyTokenWithSecret(token, userBadge.SecretKey);
        if (verify.Result.IsValid)
            return new Securoman.AuthenticateResult(userBadge.RolePrincipal,
                verify.Claims.Select(x => new Claim(x.Type, x.Value/*, x.Issuer*/)));
        if (!verify.HasInvalidSecretKey)
            return new Securoman.AuthenticateResult(verify.Result.Exception.Message);
        await RemoveUserAsync(new Guid(userId));
        return await Authenticate(token);
    }
    
    /*public async Task<TokenResult?> Authenticate()
    {
        if (_httpContextAccessor.HttpContext == null)
            return null;
        if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(SecuromanDefaults.SessionCookie, out _)
            && _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(SecuromanDefaults.TokenCookie, out var token))
        {
            var payload = Securoman.GetUnverifiedUserTicket(token);
            var userId = payload?.FirstOrDefault(x=>x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return null;
            var userBadge = await GetUser(new Guid(userId));
            if (userBadge == null)
            {
                try
                {
                    var userBadgerRequest = _securomanUrl
                        .WithCookies(_httpContextAccessor.HttpContext.Request.Cookies)
                        .AppendPathSegment("api/v1/auth")
                        .AppendPathSegment("refreshBadge");
                    var updatedUserBadge = await userBadgerRequest.GetJsonAsync<UserBadge>();
                    if (updatedUserBadge == null) return null;
                    await SetOrUpdateUserAsync(updatedUserBadge);
                    userBadge = updatedUserBadge;
                }
                catch (FlurlHttpException ex)
                {
                    return null;
                }
            
            }
            var verify = Securoman.VerifyTokenWithSecret(token, userBadge.SecretKey);
            if (verify.Result.IsValid)
            {
                _httpContextAccessor.HttpContext.User.AddIdentity(new ClaimsIdentity(verify.Claims.Select(x => new Claim(x.Type, x.Value)), "Securoman"));
                _httpContextAccessor.HttpContext.User.AddIdentity(new ClaimsIdentity(userBadge.RolePrincipal.Roles.Select(
                    x => new Claim(ClaimTypes.Role, x)), "Securoman"));
                _httpContextAccessor.HttpContext.User.AddIdentity(new ClaimsIdentity(userBadge.RolePrincipal.Permissions.Select(
                    x => new Claim(x.Type, x.Value,x.Issuer)), "Securoman"));
                return verify;
            }
            if (!verify.HasInvalidSecretKey) return null;
            await RemoveUserAsync(new Guid(userId));
            return await Authenticate();
        }

        return null;
    }*/

    public Task SetOrUpdateUserAsync(UserBadge badge)
    { 
        var options = new DistributedCacheEntryOptions();
        options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
        return _cache.SetAsync(badge.UserId.ToString(), SerializeBadgeToBytes(badge), options);
    }
    
    public Task RemoveUserAsync(Guid userId)
    { 
        return _cache.RemoveAsync(userId.ToString());
    }
    private async Task<UserBadge?> GetUser(Guid userId)
    {
        var key = userId.ToString();
        var rawCacheData = await _cache.GetAsync(key);
        if(rawCacheData == null || rawCacheData.Length == 0)  return null;
        var userBadgeCacheData = ToUserBadgeCacheData(rawCacheData);
        if (userBadgeCacheData == null) return null;
        _cache.RefreshAsync(key);
        return new UserBadge()
        {
            LogHandleId = userBadgeCacheData.LogHandleId,
            SecretKey = userBadgeCacheData.SecretKey,
            UserId = userId,
            RolePrincipal = userBadgeCacheData.RolePrincipal
        };
    }
    
    public async Task<UserBadge?> GetUserFromSecuromanCache(Guid userId)
    {
        var key = userId.ToString();
        var rawCacheData = await _cache.GetAsync(key);
        if (rawCacheData.Length == 0)
            return null;
        var userBadgeCacheData = ToUserBadgeCacheData(rawCacheData);
        if (userBadgeCacheData == null) return null;
        return new UserBadge()
        {
            LogHandleId = userBadgeCacheData.LogHandleId, 
            UserId = userId,
            RolePrincipal = userBadgeCacheData.RolePrincipal
        };
    }
    
    private static byte[] SerializeBadgeToBytes(UserBadge badge)
    {
        var badgeStore = new UserBadgeCacheData()
        {
            LogHandleId = badge.LogHandleId,
            SecretKey = badge.SecretKey,
            RolePrincipal = badge.RolePrincipal
        };
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(badgeStore));
    }
    private static byte[] SerializeBadgeToBytes(UserBadgeCacheData badgeCacheData)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(badgeCacheData));
    }
    private static UserBadgeCacheData? ToUserBadgeCacheData(byte[] source)
    {
        return source == null ? null : JsonSerializer.Deserialize<UserBadgeCacheData>(Encoding.UTF8.GetString(source));
    }
}