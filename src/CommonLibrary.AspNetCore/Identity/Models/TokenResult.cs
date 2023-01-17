using Paseto;

namespace CommonLibrary.AspNetCore.Identity;

public class TokenResult
{
    public PasetoTokenValidationResult Result { get; }

    public TokenResult(PasetoTokenValidationResult result, byte[] publicKey, IEnumerable<Securoman.TicketClaim> claims)
    {
        Result = result;
        PublicKey = publicKey;
        Claims = claims;
    }
    public TokenResult(PasetoTokenValidationResult result)
    {
        Result = result;
    }
    public byte[] PublicKey { get; }
    public IEnumerable<Securoman.TicketClaim> Claims { get; set; }
}