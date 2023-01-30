using System.Text;
using System.Text.Json;
using CommonLibrary.Identity.Models;
using CommonLibrary.Identity.Models.Dtos;

namespace CommonLibrary.ClientServices.Identity.Helpers;

public static class Securoman
{
    public static IEnumerable<UserClaim>? GetUserClaims(string token)
    {
        var payload = DecodeToken(token);
        if (payload is null) return null;
        if(payload.TryGetValue(UserClaimTypes.UserClaims, out var ticket) &&
           payload.TryGetValue(UserClaimTypes.SessionId, out var sessionId))
        {
            var claims = new List<UserClaim>();
            claims.Add(new UserClaim(UserClaimTypes.SessionId, sessionId.ToString()/*, "UserService"*/));
            claims.AddRange(JsonSerializer.Deserialize<List<UserClaim>>(ticket.ToString()));
            return claims;
        }
        return null;
    }
    private static IDictionary<string, object>? DecodeToken(string token)
    {
        
        var tokenParts = token.Split('.');
        var bytes = FromBase64Url(tokenParts[2]);
        if (bytes.Length <= 64)
            return null;
        byte[] payload = bytes.Take(bytes.Length - 64).ToArray();
        return JsonSerializer.Deserialize<Dictionary<string, object>>(Encoding.UTF8.GetString(payload));
    }
    private static byte[] FromBase64Url(string source)
    {
        try
        {
            return Convert.FromBase64String(source.PadRight((source.Length % 4) == 0 ? 0 : (source.Length + 4 - (source.Length % 4)), '=')
                .Replace('-', '+')
                .Replace('_', '/'));
        }
        catch (FormatException e)
        {
            throw new FormatException("The base64 encoding was invalid. " + e);
        }
    }
}