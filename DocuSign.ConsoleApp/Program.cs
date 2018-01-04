using System;

namespace DocuSign.ConsoleApp {
    internal static class Program {
        private static readonly string ApiBaseUrl = "https://demo.docusign.net/restapi"; // production code should use geo-targeted server ( https://docs.docusign.com/ )
        private static readonly string ClientId = "5fa9d8e2-aac8-40f9-99d1-9e9e2d144979"; // replace this fake value with your API Integrator Key ( https://docs.docusign.com/esign/guide/authentication/integrator_key.html )
        private static readonly string UserId = "6cf1e309-76f7-4e95-b684-e56ed8d5f21e"; // replace this fake value with your API Username ( https://admindemo.docusign.com/api-integrator-key )
        private static readonly string OAuthBasePath = "account-d.docusign.com"; // production code should use account.docusign.com ( https://www.docusign.com/developer-center/api-overview )
        private static readonly string PrivateKeyText = @"-----BEGIN RSA PRIVATE KEY-----MIICXgIBAAKBgQDv6i/mxtS2B2PjShArtOAmdRoEcCWa/LH1GcrbW14zdbmIqrxb....................faXRHcG37TkvglUZ3wgy6eKuyrDi5gkwV8WAuaoNct5j5w==-----END RSA PRIVATE KEY-----"; // replace this fake value with your Integrator Key's RSA Private Key, displayed to you in plaintext only when you create each Integrator Key; header/footer text is optional here
        private static readonly int AccountId = 3587261; // replace this fake value with your Billing Account ID ( https://admindemo.docusign.com/billing )

        private static void Main() {
            AddUser();
        }

        // creating a user that already exists with a Pending status does not create an additional user
        // creating a user that already exists with a Closed status creates an additional user
        private static void AddUser() {
            var user = new AccountCreationRequestUser {userName = "C#-Created User", email = "csharptest@example.com"};
            var authorizationHeaderValue = AuthorizationHeaderValueGetter.Get(ClientId, UserId, OAuthBasePath, PrivateKeyText);
            var result = AccountCreator.Create(ApiBaseUrl, AccountId, authorizationHeaderValue, user);
            Console.WriteLine($"Created User ID: {result.newUsers[0].userId}");
            Console.ReadKey();
        }
    }
}