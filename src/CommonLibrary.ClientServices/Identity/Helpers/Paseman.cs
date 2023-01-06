using Paseto;
using Paseto.Builder;

namespace CommonLibrary.ClientServices.Identity.Helpers;

public static class Paseman
{

    public static PasetoTokenValidationResult VerifyToken(
        string token,
        byte[] publicKey,
        PasetoTokenValidationParameters? parameters = null, ProtocolVersion version = ProtocolVersion.V4)
    {
        var builder = new PasetoBuilder()
            .Use(version, Purpose.Public)
            .WithPublicKey(publicKey);
        PasetoTokenValidationResult result;
        if (parameters != null)
            result = builder.Decode(token, parameters);
        else
            result = builder.Decode(token);
        return result;
    }
}