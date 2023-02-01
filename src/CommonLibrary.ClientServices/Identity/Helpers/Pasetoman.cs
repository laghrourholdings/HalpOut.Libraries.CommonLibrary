using Paseto;
using Paseto.Builder;

namespace CommonLibrary.ClientServices.Identity;

public static class Pasetoman
{
    public static PasetoTokenValidationResult VerifyToken(
        string token,
        byte[] publicKey,
        PasetoTokenValidationParameters? parameters = null, ProtocolVersion version = ProtocolVersion.V4)
    {
        
        
        var result = new PasetoBuilder()
            .Use(version, Purpose.Public)
            .WithPublicKey(publicKey)
            .Decode(token, parameters);
       
        return result;
    }
}