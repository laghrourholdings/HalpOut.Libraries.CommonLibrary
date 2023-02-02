using CommonLibrary.Identity.Dtos;
using Paseto;

namespace CommonLibrary.AspNetCore.Identity;

public class TokenResult
{
    public PasetoTokenValidationResult Result { get; }

    public TokenResult(PasetoTokenValidationResult result, byte[] publicKey, IEnumerable<UserClaim> claims)
    {
        Result = result;
        PublicKey = publicKey;
        Claims = claims;
    }
    public TokenResult(PasetoTokenValidationResult result)
    {
        Result = result;
    }
    public TokenResult(bool invalidSecretKey)
    {
        HasInvalidSecretKey = invalidSecretKey;
    }
    public bool HasInvalidSecretKey { get; }
    public byte[] PublicKey { get; }
    public IEnumerable<UserClaim> Claims { get;}
}