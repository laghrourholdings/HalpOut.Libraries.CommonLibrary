using System.Text.Json;
using CommonLibrary.Identity.Models;
using Paseto;

namespace CommonLibrary.ClientServices.Identity.Helpers;

public static class Securoman
{
    public record TicketClaim(string Type, string Value);
    public static PasetoTokenValidationParameters DefaultParameters { get; } = new PasetoTokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidAudience = "laghrour.com",
        ValidIssuer = "auth.laghrour.com"
    };
    
    public static TokenSignature VerifyToken(string token, byte[] publicKey, PasetoTokenValidationParameters? paramms = null)
    {
        var result = Pasetoman.VerifyToken(token, publicKey, paramms ?? DefaultParameters);
        if (result.IsValid && result.Paseto.Payload.TryGetValue(UserClaimTypes.UserTicket, out var ticket))
        {
            var claims = JsonSerializer.Deserialize<IEnumerable<TicketClaim>>(ticket.ToString());
            return new TokenSignature(result, publicKey, claims);
        }
        return new TokenSignature(result);
    }

    public class TokenSignature
    {
        public PasetoTokenValidationResult Result { get; }

        public TokenSignature(PasetoTokenValidationResult result, byte[] publicKey, IEnumerable<TicketClaim> claims)
        {
            Result = result;
            PublicKey = publicKey;
            Claims = claims;
        }
        public TokenSignature(PasetoTokenValidationResult result)
        {
            Result = result;
        }
        public byte[] PublicKey { get; }
        public IEnumerable<TicketClaim> Claims { get; set; }
    }
}