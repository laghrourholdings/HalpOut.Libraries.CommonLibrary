using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using NaCl.Core.Internal;
using Paseto;
using Paseto.Builder;
using Paseto.Cryptography.Key;
using Paseto.Protocol;

namespace CommonLibrary.AspNetCore.Identity;

public static class Pasetoman
{
    public static readonly byte[] DebugSymmetryKey 
        = CryptoBytes.FromHexString("707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f");
    
    /*public class TokenSignature
    {
        public PasetoTokenValidationResult Result { get; }

        public TokenSignature(PasetoTokenValidationResult result, byte[] publicKey, Guid sessionId = default)
        {
            Result = result;
            PublicKey = publicKey;
            SessionId = sessionId;
        }
        public TokenSignature(PasetoTokenValidationResult result)
        {
            Result = result;
        }
        public byte[] PublicKey { get; }
        public Guid SessionId { get;  } 
    }*/
    public static PasetoAsymmetricKeyPair AsymmetricKeyPair(byte[] privateKey, byte[] publicKey) =>
        new(privateKey, publicKey, new Version4());
    public static PasetoAsymmetricKeyPair GenerateAsymmetricKeyPair(ProtocolVersion version = ProtocolVersion.V4) 
        => new PasetoBuilder().Use(version, Purpose.Public).GenerateAsymmetricKeyPair(RandomNumberGenerator.GetBytes(32));

    public static PasetoSymmetricKey GenerateSymmetricKey(ProtocolVersion version = ProtocolVersion.V4) 
        => new PasetoBuilder().Use(version, Purpose.Local).GenerateSymmetricKey();

    public static string EncodeTo64(string toEncode)
    {
        byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);
        return Convert.ToBase64String(toEncodeAsBytes);
    }

    public static string DecodeFrom64(string encodedData)
    {
        byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
        return Encoding.ASCII.GetString(encodedDataAsBytes);
    }
    public static PasetoBuilder CreateTokenPipe(
        string issuer,
        string audience,
        DateTime expiration)
    {
        PasetoSymmetricKey key;
        
        return new PasetoBuilder()
            .Issuer(issuer)
            .Audience(audience)
            .Expiration(expiration);
    }

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
    
    /*public static PasetoTokenValidationResult VerifyTokenSignature(
        string token,
        byte[] symmetricKey,
        PasetoTokenValidationParameters? parameters = null, ProtocolVersion version = ProtocolVersion.V4)
    {
        //TODO: Fix DecodeFooter that is returning the payload instead of the footer
        var footer = token.Split(".").Last();
        var footerDecryptResult = new PasetoBuilder()
            .Use(version, Purpose.Local)
            .WithSharedKey(symmetricKey)
            .Decode(footer);
        if (!footerDecryptResult.IsValid)
            return new PasetoTokenValidationResult(footerDecryptResult);
        var footerClaims = footerDecryptResult.Paseto.Payload.ToDictionary(x => x.Key, x => x.Value);
        var pk = CryptoBytes.FromHexString(footerClaims["pk"].ToString());
        var tokenVerifResult = VerifyToken(
            token,
            pk,
            parameters);
        return tokenVerifResult;
    }*/

    public static string Sign(
        this PasetoBuilder _builder,
        byte[] secretKey,
        ProtocolVersion version = ProtocolVersion.V4) =>
        _builder
            .Use(version, Purpose.Public)
            .WithSecretKey(secretKey)
            .Encode();

    /*public static string Encrypt(this PasetoBuilder _builder, 
        byte[] symmetricKey, 
        ProtocolVersion version = ProtocolVersion.V4) =>
            _builder.Use(version, Purpose.Local)
            .WithSharedKey(symmetricKey)
            .Encode();
    
    public static PasetoTokenValidationResult Decrypt(
        this PasetoBuilder _builder,
        string token,
        byte[] symmetricKey,
        PasetoTokenValidationParameters? parameters = null,
        ProtocolVersion version = ProtocolVersion.V4) =>
            _builder.Use(version, Purpose.Local)
            .WithSharedKey(symmetricKey)
            .Decode(token, parameters);*/
    
    
    public static string DecodePayload(string token, byte[] publicKey, ProtocolVersion version = ProtocolVersion.V4) =>
        new PasetoBuilder()
            .Use(version, Purpose.Public)
            .WithPublicKey(publicKey)
            .DecodePayload(token);

    public static string DecodeFooter(string token, ProtocolVersion version = ProtocolVersion.V4) =>
        new PasetoBuilder()
            .DecodeFooter(token);

    public static string DecodeHeader(string token, ProtocolVersion version = ProtocolVersion.V4) =>
        new PasetoBuilder()
            .DecodeHeader(token);
    
    public static byte[] SerializeToBytes(AuthenticationTicket source)
    {
        return TicketSerializer.Default.Serialize(source);
    }
    
    public static AuthenticationTicket? DeserializeFromBytes(byte[] source)
    {
        return source == null ? null : TicketSerializer.Default.Deserialize(source);
    }
}