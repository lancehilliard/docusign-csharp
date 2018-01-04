using System.Collections.Generic;

namespace DocuSign.ConsoleApp {
    public static class AuthorizationHeaderValueGetter {
        public static string Get(string clientId, string userId, string oAuthBasePath, string privateKeyText) {
            var accessToken = AccessTokenCreator.Create(clientId, userId, oAuthBasePath, privateKeyText);
            var baseUrl = $"https://{oAuthBasePath}";
            var resource = "oauth/token";
            var headerParams = new Dictionary<string, string> {{"Content-Type", "application/x-www-form-urlencoded"}};
            var formParams = new Dictionary<string, string> {{"grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer"}, {"assertion", accessToken}};
            var response = RestResponseGetter<OAuthTokenResponse>.GetPostResponse(baseUrl, resource, headerParams, formParams);
            var result = $"{response.token_type} {response.access_token}";
            return result;
        }
    }
}