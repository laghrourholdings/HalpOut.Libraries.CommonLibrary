using System.Security.Claims;
using System.Text;
using System.Text.Json;
using CommonLibrary.Identity.Models;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Paseto;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace CommonLibrary.AspNetCore.Identity;

public interface ISecuromanService
{
    Task<TokenResult?> Identify(string token);
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
    }

    public SecuromanService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _cache = Securoman.GetSecuromanCache(config);
        _securomanUrl = Securoman.GetSecuromanUrl(config);
    }

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

    public async Task<TokenResult?> Identify(string token)
    {
        var payload = Securoman.GetUnverifiedUserTicket(token);
        var userId = payload?.FirstOrDefault(x=>x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return null;
        var userBadge = await GetUser(new Guid(userId));
        if (userBadge == null)
        {
            userBadge = await _securomanUrl
                .SetQueryParam("token",token)
                .WithCookies(_httpContextAccessor.HttpContext?.Request.Cookies)
                .GetJsonAsync<UserBadge>();
            if(userBadge == null) return null;
            await SetOrUpdateUserAsync(userBadge);
        }
        var verify = Securoman.VerifyTokenWithSecret(token, userBadge.SecretKey);
        return verify;
    }

    public Task SetOrUpdateUserAsync(UserBadge badge)
    { 
        var options = new DistributedCacheEntryOptions();
        options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(90);
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
        if (rawCacheData.Length == 0)
            return null;
        var userBadgeCacheData = ToUserBadgeCacheData(rawCacheData);
        if (userBadgeCacheData == null) return null;
        
        _cache.RefreshAsync(key);
        return new UserBadge()
        {
            LogHandleId = userBadgeCacheData.LogHandleId,
            SecretKey = userBadgeCacheData.SecretKey,
            UserId = userId
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
            UserId = userId
        };
    }
    
    private static byte[] SerializeBadgeToBytes(UserBadge badge)
    {
        var badgeStore = new UserBadgeCacheData()
        {
            LogHandleId = badge.LogHandleId,
            SecretKey = badge.SecretKey
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