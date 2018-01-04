using System.Collections.Generic;

namespace DocuSign.ConsoleApp {
    public static class AccountCreator {
        public static AccountCreationResponse Create(string apiBaseUrl, int accountId, string authorizationHeaderValue, AccountCreationRequestUser user) {
            var resource = $"v2/accounts/{accountId}/users";
            var headerParams = new Dictionary<string, string> {{"Content-Type", "application/json"}, {"Authorization", authorizationHeaderValue}};
            var users = new List<AccountCreationRequestUser> {user};
            var postBody = new Dictionary<string, object> {{"newUsers", users}};
            var result = RestResponseGetter<AccountCreationResponse>.GetPostResponse(apiBaseUrl, resource, headerParams, body: postBody);
            return result;
        }
    }
}