using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CommonLibrary.Identity.Models;
using Paseto;
using Paseto.Cryptography.Key;

namespace CommonLibrary.AspNetCore.Identity;

public static class Securoman
{
    public record UserClaim(string Type, string Value/*, string Issuer*/);
    public class AuthenticateResult
    {
        public bool Succeeded { get;}
        public string? ErrorMessage { get; }
        public List<RoleIdentity>? RolePrincipal { get; } = new();
        public IEnumerable<Claim>? Claims { get;}
        public AuthenticateResult(string errorMessage)
        {
            Succeeded = false;
            ErrorMessage = errorMessage;
        }
        public AuthenticateResult(List<RoleIdentity> rolePrincipal, IEnumerable<Claim> claims)
        {
            Succeeded = true;
            RolePrincipal = rolePrincipal;
            Claims = claims;
        }
    }
    public static PasetoTokenValidationParameters DefaultParameters { get; } = new PasetoTokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidAudience = "laghrour.com",
        ValidIssuer = "auth.laghrour.com"
    };
    
       
    private static byte[] AES_Encrypt(
        byte[] plain,
        byte[] sharedKey,
        byte[] salt)
    {
        if (plain == null || plain.Length <= 0)
            throw new ArgumentNullException("plain");
        if (sharedKey == null || sharedKey.Length <= 0)
            throw new ArgumentNullException("sharedKey");
        if (salt == null || salt.Length <= 0)
            throw new ArgumentNullException("salt");
        
        byte[] encryptedBytes;
        
        
        using (MemoryStream ms = new MemoryStream())
        {
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                AES.KeySize = 256;
                AES.BlockSize = 128;

                var key = new Rfc2898DeriveBytes(sharedKey, salt, 1000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);

                AES.Mode = CipherMode.CBC;

                using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(plain, 0, plain.Length);
                    cs.Close();
                }
                encryptedBytes = ms.ToArray();
            }
        }

        return encryptedBytes;
    }
    private static byte[] AES_Decrypt(
        byte[] cipher,
        byte[] sharedKey,
        byte[] salt)
    {
        if (cipher == null || cipher.Length <= 0)
            throw new ArgumentNullException("bytesToBeEncrypted");
        if (sharedKey == null || sharedKey.Length <= 0)
            throw new ArgumentNullException("sharedKey");
        if (salt == null || salt.Length <= 0)
            throw new ArgumentNullException("salt");
        byte[] decryptedBytes;

        using (MemoryStream ms = new MemoryStream())
        {
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                AES.KeySize = 256;
                AES.BlockSize = 128;

                var key = new Rfc2898DeriveBytes(sharedKey, salt, 1000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);

                AES.Mode = CipherMode.CBC;

                using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipher, 0, cipher.Length);
                    cs.Close();
                }
                decryptedBytes = ms.ToArray();
            }
        }

        return decryptedBytes;
    }
    
    private static string EncryptPublicKey(byte[] decryptedPublicKey, byte[] symmetricKey, string? sessionId = null)
    {
        // Get the bytes of the string
        byte[] IV;
        IV = sessionId is null ? new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 2, 3 } : Encoding.UTF8.GetBytes(sessionId);

        // Hash the password with SHA256
        var sharedKey = SHA256.Create().ComputeHash(symmetricKey);

        byte[] bytesEncrypted = AES_Encrypt(decryptedPublicKey, sharedKey, IV);
        
        
        return Convert.ToHexString(bytesEncrypted);
    }
    
    private static byte[] DecryptPublicKey(byte[] encryptedPublicKey, byte[] symmetricKey, string? sessionId = null)
    {
        // Get the bytes of the string
        byte[] IV;
        IV = sessionId is null ? new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 2, 3 } : Encoding.UTF8.GetBytes(sessionId);

        // Hash the password with SHA256
        var sharedKey = SHA256.Create().ComputeHash(symmetricKey);

        byte[] bytesDecrypted = AES_Decrypt(encryptedPublicKey, sharedKey, IV);
        
        return bytesDecrypted;
    }
    
    public static string GenerateToken(
        PasetoAsymmetricKeyPair keyPair,
        IEnumerable<Claim> claims,
        byte [] symmetricKey,
        Guid sessionId,
        DateTimeOffset exp)
    {
        var publicKey = keyPair.PublicKey.Key.ToArray();
        var secretKey = keyPair.SecretKey.Key.ToArray();
        
        //TODO refactor to not have hardcoded stuff
        var tokenBuilder = Pasetoman.CreateTokenPipe("auth.laghrour.com","laghrour.com",exp.DateTime);
        tokenBuilder.AddClaim(UserClaimTypes.SessionId, sessionId.ToString());
        tokenBuilder.AddClaim(UserClaimTypes.Ticket, JsonSerializer.Serialize(claims.Select(x => new UserClaim(x.Type,x.Value/*, x.Issuer*/))));
        tokenBuilder.AddFooter(EncryptPublicKey(publicKey, symmetricKey));
        
        
        return tokenBuilder.Sign(secretKey);
    }

    public static TokenResult VerifyTokenWithSecret(string token, byte[] SymmetryKey, PasetoTokenValidationParameters? paramms = null)
    {
        try
        {
            var footer = Pasetoman.DecodeFooter(token); 
            var publicKey = DecryptPublicKey(Convert.FromHexString(footer), SymmetryKey);
            if (publicKey == null || publicKey.Length == 0)
                return new TokenResult(true);
            return VerifyToken(token, publicKey, paramms);
        }catch(Exception e)
        {
            return new TokenResult(true);
        }
    }
    
    public static TokenResult VerifyToken(string token, byte[] publicKey, PasetoTokenValidationParameters? paramms = null)
    {
        var result = Pasetoman.VerifyToken(token, publicKey, paramms ?? DefaultParameters);
        if (result.IsValid 
            && result.Paseto.Payload.TryGetValue(UserClaimTypes.Ticket, out var ticket)
            && result.Paseto.Payload.TryGetValue(UserClaimTypes.SessionId, out var sessionId))
        {
            var claims = new List<UserClaim>();
            claims.Add(new UserClaim(UserClaimTypes.SessionId, sessionId.ToString()/*, "UserService"*/));
            claims.AddRange(JsonSerializer.Deserialize<IEnumerable<UserClaim>>(ticket.ToString()));
            return new TokenResult(result, publicKey, claims);
        }
        return new TokenResult(result);
    }

    public static IEnumerable<UserClaim>? GetUnverifiedUserTicket(string token)
    {
        var payload = UnsecurePayloadDecode(token);
        if (payload is null) return null;
        if(payload.TryGetValue(UserClaimTypes.Ticket, out var ticket) &&
           payload.TryGetValue(UserClaimTypes.SessionId, out var sessionId))
        {
            var claims = new List<UserClaim>();
            claims.Add(new UserClaim(UserClaimTypes.SessionId, sessionId.ToString()/*, "UserService"*/));
            claims.AddRange(JsonSerializer.Deserialize<List<UserClaim>>(ticket.ToString()));
            return claims;
        }
        return null;
    }

    private static IDictionary<string, object>? UnsecurePayloadDecode(string token)
    {
        
        var tokenParts = token.Split('.');
        var bytes = FromBase64Url(tokenParts[2]);
        if (bytes.Length <= 64)
            return null;
        byte[] payload = bytes.Take(bytes.Length - 64).ToArray();
        return JsonSerializer.Deserialize<Dictionary<string, object>>(Encoding.UTF8.GetString(payload));
    }

    public static byte[] PreAuthEncode(IReadOnlyList<byte[]> pieces) =>
        BitConverter.GetBytes((ulong) pieces.Count)
            .Concat(pieces.SelectMany(piece => BitConverter.GetBytes((ulong) piece.Length).Concat(piece)))
            .ToArray();

    public static string ToBase64Url(IEnumerable<byte> source) =>
        Convert.ToBase64String(source.ToArray())
            .Replace("=", "")
            .Replace('+', '-')
            .Replace('/', '_');

    // Replace some characters in the base 64 string and add padding so .NET can parse it
    public static byte[] FromBase64Url(string source)
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