using System;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.IO;
using System.Security.Claims;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace DocuSign.ConsoleApp {
    public static class AccessTokenCreator {
        private static readonly string KeyTextHeader = "-----BEGIN RSA PRIVATE KEY-----";
        private static readonly string KeyTextFooter = "-----END RSA PRIVATE KEY-----";
        private static readonly JwtSecurityTokenHandler SecurityTokenHandler = new JwtSecurityTokenHandler();

        public static string Create(string issuer, string subject, string audience, string privateKeyText) {
            privateKeyText = GetPrivateKeyWithHeaderAndFooter(privateKeyText);
            var rsaSecurityKey = GetRsaSecurityKey(privateKeyText);
            var tokenDescriptor = GetTokenDescriptor(issuer, subject, audience, rsaSecurityKey);
            var token = SecurityTokenHandler.CreateToken(tokenDescriptor);
            var result = SecurityTokenHandler.WriteToken(token);
            return result;
        }

        private static SecurityTokenDescriptor GetTokenDescriptor(string issuer, string subject, string audience, SecurityKey rsaSecurityKey) {
            var result = new SecurityTokenDescriptor {
                Lifetime = new Lifetime(DateTime.UtcNow, DateTime.UtcNow.AddHours(1)),
                Subject = new ClaimsIdentity()
            };
            result.Subject.AddClaim(new Claim("iss", issuer));
            result.Subject.AddClaim(new Claim("sub", subject));
            result.Subject.AddClaim(new Claim("aud", audience));
            result.Subject.AddClaim(new Claim("scope", "signature"));
            result.SigningCredentials = new SigningCredentials(rsaSecurityKey, "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256");
            return result;
        }

        private static RsaSecurityKey GetRsaSecurityKey(string privateKeyText) {
            privateKeyText = GetPrivateKeyWithHeaderAndFooter(privateKeyText);
            var asymmetricCipherKeyPair = (AsymmetricCipherKeyPair) new PemReader(new StringReader(privateKeyText)).ReadObject();
            var rsaKeyFromPem = DotNetUtilities.ToRSA((RsaPrivateCrtKeyParameters) asymmetricCipherKeyPair.Private);
            var result = new RsaSecurityKey(rsaKeyFromPem);
            return result;
        }

        private static string GetPrivateKeyWithHeaderAndFooter(string privateKeyText) {
            var result = privateKeyText.Replace(KeyTextHeader, string.Empty);
            result = result.Replace(KeyTextFooter, string.Empty);
            result = $"{KeyTextHeader}{Environment.NewLine}{result}{Environment.NewLine}{KeyTextFooter}";
            return result;
        }
    }
}