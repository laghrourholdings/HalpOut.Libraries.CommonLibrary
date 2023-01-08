using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Paseto;
using Paseto.Cryptography.Key;

namespace CommonLibrary.AspNetCore.Identity.Helpers;

public static class Securoman
{
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
        IEnumerable<Claim> claims,string sessionId)
    {
        var publicKey = keyPair.PublicKey.Key.ToArray();
        var secretKey = keyPair.SecretKey.Key.ToArray();
        
        //TODO refactor to not have hardcoded stuff
        var tokenBuilder = PSec.CreateTokenPipe("auth.laghrour.com","laghrour.com",DateTime.UtcNow.AddMinutes(5));
        tokenBuilder.AddClaim("sid", sessionId);
        tokenBuilder.AddFooter(EncryptPublicKey(publicKey, PSec.DebugSymmetryKey));
        foreach (var claim in claims)
        {
            tokenBuilder.AddClaim(claim.Type, claim.Value);
        }
        
        return tokenBuilder.Sign(secretKey);
    }

    public static TokenSignature VerifyToken(string token)
    {
        var footer = PSec.DecodeFooter(token);
        var publicKey = DecryptPublicKey(Convert.FromHexString(footer), PSec.DebugSymmetryKey);
        
        return new TokenSignature(PSec.VerifyToken(token, publicKey), publicKey);
    }

    public class TokenSignature
    {
        public PasetoTokenValidationResult Result { get; }

        public TokenSignature(PasetoTokenValidationResult result, byte[] publicKey, Guid sessionId = default)
        {
            Result = result;
            PublicKey = publicKey;
        }
        public TokenSignature(PasetoTokenValidationResult result)
        {
            Result = result;
        }
        public byte[] PublicKey { get; }
    }
}