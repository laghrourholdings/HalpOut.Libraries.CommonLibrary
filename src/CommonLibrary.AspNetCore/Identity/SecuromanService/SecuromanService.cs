
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using CommonLibrary.AspNetCore.Core;
using CommonLibrary.Identity.Models;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
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
    //Guid? GetUnverifiedLogHandleId(string token);
    public string GetSecuromanUrl();
    public bool HasPermission(string permission);
    public bool IsInRole(string roleName);
    public UserPermission? GetPermission(string permission);
    public Guid GetLogHandleId();
    public Guid GetUserId();
}

public class SecuromanService : ISecuromanService
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
        public List<RoleIdentity> RolePrincipal { get; set; }
    }

    public SecuromanService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        _config = config;
        _httpContextAccessor = httpContextAccessor;
        _cache = GetSecuromanCache();
        _securomanUrl = GetSecuromanUrl();
        //_securomanGrpcUrl = GetUserServiceGrpcUrl();
    }

    public bool HasPermission(string permission)
    {
        var rolePrincipal = GetContextRolePrincipal();
        if (!IsAuthenticated() || rolePrincipal  == null) return false;
        return rolePrincipal.Any(x=>x.Name == permission);
    }

    public bool IsInRole(string roleName)
    {
        var rolePrincipal = GetContextRolePrincipal();
        if (!IsAuthenticated() || rolePrincipal  == null) return false;
        return rolePrincipal.Any(x=>x.Name == roleName);
    }

    public UserPermission? GetPermission(string permission)
    {
        var rolePrincipal = GetContextRolePrincipal();
        if (!IsAuthenticated() || rolePrincipal  == null) return null;
        return rolePrincipal.SelectMany(x=>x.Permissions).SingleOrDefault(x => x.Value == permission);
    }
    
    private List<RoleIdentity>? GetContextRolePrincipal()
        => _httpContextAccessor.HttpContext?.Items[nameof(RoleIdentity)] as List<RoleIdentity>;



    public Guid GetLogHandleId() => 
        new(_httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == UserClaimTypes.LogHandleId).Value);

    public Guid GetUserId() => 
        new(_httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == UserClaimTypes.Id).Value);

    public bool IsAuthenticated() 
        => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == UserClaimTypes.Id) != null;

    public async Task<Securoman.AuthenticateResult> Authenticate(string token)
    {
        var payload = Securoman.GetUnverifiedUserTicket(token);
        var userId = payload?.FirstOrDefault(x => x.Type == UserClaimTypes.Id)?.Value;
        if (userId == null) return new Securoman.AuthenticateResult("UserId is null");
        var userBadge = await GetUser(new Guid(userId));
        if (userBadge == null)
        {
            try
            {
                //var channel = GrpcChannel.ForAddress(_securomanGrpcUrl);
                var userBadgerRequest = _securomanUrl
                    .WithCookies(_httpContextAccessor.HttpContext?.Request.Cookies)
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
                verify.Claims.Select(x => new Claim(x.Type, x.Value /*, x.Issuer*/)));
        if (!verify.HasInvalidSecretKey)
            return new Securoman.AuthenticateResult(verify.Result.Exception.Message);
        await RemoveUserAsync(new Guid(userId));
        return await Authenticate(token);
    }

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

    private IDistributedCache GetSecuromanCache()
        => new RedisCache(new RedisCacheOptions
        {
            Configuration = _config.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>().SecuromanCacheConfiguration
        });

    public string GetSecuromanUrl() 
        => _config.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>().UserServiceUrl 
           ?? throw new InvalidOperationException("Securoman url is inexistant");
    public string GetUserServiceGrpcUrl() 
        => _config.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>().UserServiceGrpcUrl 
           ?? throw new InvalidOperationException("Securoman grpc url is inexistant");
}