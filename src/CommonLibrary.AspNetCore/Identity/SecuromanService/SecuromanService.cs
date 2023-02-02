
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using CommonLibrary.AspNetCore.Core;
using CommonLibrary.Identity;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace CommonLibrary.AspNetCore.Identity;

public interface ISecuroman
{
    Task<SecuromanTokenizer.AuthenticateResult> Authenticate(string token);
    //Task<TokenResult?> Authenticate();
    bool IsAuthenticated();
    // Task SetOrUpdateUserAsync(UserBadge badge);
    Task RemoveUserAsync(Guid userId);
    // Task<UserBadge?> GetUserFromSecuromanCache(Guid userId);
    //Guid? GetUnverifiedLogHandleId(string token);
    public bool HasPermission(string permission);
    public bool HasPrivilege(string privilege);
    public bool HasRight(string right);
    public bool HasRole(string roleName);
    //public Task<RoleIdentity?> GetRole(string roleName);
    //public Task<List<RoleIdentity>?> GetRoles();
    public Guid GetLogHandleId();
    public Guid GetUserId();
    public string? GetClaim(string type);
    public IEnumerable<string>? GetClaims(string type);
    public string GetSecuromanUrl();
}

public class Securoman : ISecuroman
{
    private readonly IConfiguration _config;
    private readonly IDistributedCache _cache;
    private readonly string _securomanUrl;
    private readonly IHttpContextAccessor _httpContextAccessor;
    //private readonly string _securomanGrpcUrl;

    public class UserBadgeCacheData
    {
        public Guid LogHandleId { get; set; }
        public byte[] SecretKey { get; set; }
        //public List<RoleIdentity> RolePrincipal { get; set; }
    }

    public Securoman(IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        _config = config;
        _httpContextAccessor = httpContextAccessor;
        _cache = GetSecuromanCache();
        _securomanUrl = GetSecuromanUrl();
        //_securomanGrpcUrl = GetUserServiceGrpcUrl();
    }

    

    public bool HasRole(string role)
    {
        return IsAuthenticated()
               && _httpContextAccessor.HttpContext!.User.HasClaim(UserClaimTypes.Role, role);
    }
    public bool HasRight(string right)
    {
        return IsAuthenticated()
               && _httpContextAccessor.HttpContext!.User.HasClaim(UserClaimTypes.Right, right);
    }
    
    public bool HasPrivilege(string privilege)
    {
        return IsAuthenticated() 
               && _httpContextAccessor.HttpContext!.User.HasClaim(UserClaimTypes.Privilege, privilege);
    }
    
    public bool HasPermission(string permission)
    {
        return IsAuthenticated() && 
               (_httpContextAccessor.HttpContext!.User.HasClaim(UserClaimTypes.Right, permission) 
                || _httpContextAccessor.HttpContext.User.HasClaim(UserClaimTypes.Privilege, permission));
    }
    
    public string? GetClaim(string type)
    {
        return !IsAuthenticated() ? null : _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == type)?.Value; 
    }
    //TODO: add TryGet...
    public IEnumerable<string>? GetClaims(string type)
    {
        return !IsAuthenticated()
            ? null
            : _httpContextAccessor.HttpContext!.User.Claims.Where(x => x.Type == type).Select(x => x.Value);
    }

    // private List<RoleIdentity>? GetContextRolePrincipal()
    //     => _httpContextAccessor.HttpContext?.Items[SecuromanDefaults.ContextRolePrincipal] as List<RoleIdentity>;
    
    // private List<string>? GetContextRoles()
    //     => _httpContextAccessor.HttpContext?.Items[SecuromanDefaults.ContextRoles] as List<string>;
    //
    // private List<string>? GetContextPermissions()
    //     => _httpContextAccessor.HttpContext?.Items[SecuromanDefaults.ContextPermissions] as List<string>;

    
    public Guid GetLogHandleId() => 
        new(GetClaim(UserClaimTypes.LogHandleId)!);

    public Guid GetUserId() => 
        new(GetClaim(UserClaimTypes.Id)!);

    public bool IsAuthenticated() 
        => _httpContextAccessor.HttpContext?.User.Identity != null 
           && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

    public async Task<SecuromanTokenizer.AuthenticateResult> Authenticate(string token)
    {
        var payload = SecuromanTokenizer.GetUnverifiedUserClaims(token);
        var userId = payload?.FirstOrDefault(x => x.Type == UserClaimTypes.Id)?.Value;
        if (userId == null) return new SecuromanTokenizer.AuthenticateResult("UserId is null");
        var userBadge = await GetUser(new Guid(userId));
        if (userBadge == null)
        {
            try
            {
                //var channel = GrpcChannel.ForAddress(_securomanGrpcUrl);
                /*try
                {
                    token = await GetSecuromanUrl()
                        .WithHeader("User-Agent", _httpContextAccessor.HttpContext.Request.Headers.UserAgent)
                        .WithCookies(_httpContextAccessor.HttpContext.Request.Cookies)
                        .AppendPathSegment("api/v1/token")
                        .AppendPathSegment("refreshToken")
                        .GetStringAsync();
                    if(token==null)
                        return new SecuromanTokenizer.AuthenticateResult("Unauthorized", false);
                    _httpContextAccessor.HttpContext.Response.Cookies.Append(SecuromanDefaults.TokenCookie, token,
                         new CookieOptions
                        {
                            Expires = new DateTimeOffset(2038, 1, 1, 0, 0, 0, TimeSpan.FromHours(0)),
                            Secure = true
                        });
                }catch (FlurlHttpException ex)
                {
                    return new SecuromanTokenizer.AuthenticateResult(ex.Message, false);
                }*/
                var userBadgerRequest = _securomanUrl
                    .WithCookies(_httpContextAccessor.HttpContext?.Request.Cookies)
                    .AppendPathSegment("api/v1/token")
                    .AppendPathSegment("refreshBadge");
                var updatedUserBadge = await userBadgerRequest.GetJsonAsync<UserBadge>();
                if (updatedUserBadge == null) return new SecuromanTokenizer.AuthenticateResult("Failed to grab user badge");
                await SetOrUpdateUserAsync(updatedUserBadge);
                userBadge = updatedUserBadge;
            }
            catch (FlurlHttpException ex)
            {
                return new SecuromanTokenizer.AuthenticateResult(ex.Message);
            }
        }

        var verify = SecuromanTokenizer.VerifyTokenWithSecret(token, userBadge.SecretKey);
        if (verify.Result.IsValid)
            return new SecuromanTokenizer.AuthenticateResult(/*userBadge.RolePrincipal,*/
                verify.Claims.Select(x => new Claim(x.Type, x.Value)));
        if (!verify.HasInvalidSecretKey)
            return new SecuromanTokenizer.AuthenticateResult(verify.Result.Exception.Message);
        await RemoveUserAsync(new Guid(userId));
        return await Authenticate(token);
    }

    private Task SetOrUpdateUserAsync(UserBadge badge)
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
            //RolePrincipal = userBadgeCacheData.RolePrincipal
        };
    }
    
    private async Task<UserBadge?> GetUserFromSecuromanCache(Guid userId)
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
            //RolePrincipal = userBadgeCacheData.RolePrincipal
        };
    }
    
    private static byte[] SerializeBadgeToBytes(UserBadge badge)
    {
        var badgeStore = new UserBadgeCacheData()
        {
            LogHandleId = badge.LogHandleId,
            SecretKey = badge.SecretKey,
            //RolePrincipal = badge.RolePrincipal
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

    private IDistributedCache GetSecuromanCache()
        => new RedisCache(new RedisCacheOptions
        {
            Configuration = _config.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>().SecuromanCacheConfiguration
        });

    public string GetSecuromanUrl() 
        => _config.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>().UserServiceUrl 
           ?? throw new InvalidOperationException("Securoman url is inexistant");
    // private string GetUserServiceGrpcUrl() 
    //     => _config.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>().UserServiceGrpcUrl 
    //        ?? throw new InvalidOperationException("Securoman grpc url is inexistant");
}