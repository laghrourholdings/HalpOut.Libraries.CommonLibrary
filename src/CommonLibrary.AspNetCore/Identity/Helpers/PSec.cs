using System.Security.Cryptography;
using System.Text;
using NaCl.Core.Internal;
using Paseto;
using Paseto.Builder;
using Paseto.Cryptography.Key;

namespace CommonLibrary.AspNetCore.Identity.Helpers;

public static class PSec
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
    
    public static PasetoAsymmetricKeyPair GenerateAsymmetricKeyPair(ProtocolVersion version = ProtocolVersion.V4)
    {
        return new PasetoBuilder().Use(version, Purpose.Public).GenerateAsymmetricKeyPair(RandomNumberGenerator.GetBytes(32));
    }
    
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
    
    
    public static string DecodePayload(string token, ProtocolVersion version = ProtocolVersion.V4) =>
        new PasetoBuilder()
            .Use(version, Purpose.Public)
            .DecodePayload(token);

    public static string DecodeFooter(string token, ProtocolVersion version = ProtocolVersion.V4) =>
        new PasetoBuilder()
            .DecodeFooter(token);

    public static string DecodeHeader(string token, ProtocolVersion version = ProtocolVersion.V4) =>
        new PasetoBuilder()
            .DecodeHeader(token);
}


/*
 var session = authDbContext.UserSessions.Include(x=>x.Device).SingleOrDefault(x=>x.CacheKey == key);
                        if (session != null)
                        {
 var rsa = RSA.Create();
Console.WriteLine(ASCIIEncoding.UTF8.GetString(rsa.ExportRSAPrivateKey()));
Console.WriteLine("PUBLIC:");
Console.WriteLine(ASCIIEncoding.UTF8.GetString(rsa.ExportRSAPublicKey()));
Console.WriteLine("PUBLIC:");
var token = JwtBuilder.Create()
    .WithAlgorithm(new RS256Algorithm(rsa,rsa))
    .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
    .AddClaim("Session",session.Id.ToString())
    .AddClaims(ticket.Principal.Claims.Select(x=>new KeyValuePair<string, object>(x.Type, x.Value)))
    .WithSecret("secret")
    .Encode();
httpContext.Response.Cookies.Append("Identity.Token",
    token, new CookieOptions
    {
        Expires = DateTimeOffset.UtcNow.AddHours(1),
        HttpOnly = true
    });
session.Token = token;
session.PrivateKey = rsa.ExportRSAPrivateKey();
session.PublicKey = rsa.ExportRSAPublicKey();

var rsa2 = RSA.Create();
//rsa2.ImportRSAPrivateKey(rsa.ExportRSAPrivateKey(), out _);
//rsa2.ImportRSAPublicKey(rsa.ExportRSAPublicKey(), out _);
try
{
    IJsonSerializer serializer = new JsonNetSerializer();
    IDateTimeProvider provider = new UtcDateTimeProvider();
    IJwtValidator validator = new JwtValidator(serializer, provider);
    IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
    IJwtAlgorithm algorithm = new RS256Algorithm(rsa2);
    IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

    var json = decoder.Decode(token);
    Console.WriteLine(json);
}
catch (TokenNotYetValidException)
{
    Console.WriteLine("Token is not valid yet");
}
catch (TokenExpiredException)
{
    Console.WriteLine("Token has expired");
}
catch (SignatureVerificationException)
{
    Console.WriteLine("Token has invalid signature");
}
}*/