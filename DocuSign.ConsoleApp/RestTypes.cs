namespace DocuSign.ConsoleApp {
    // https://demo.docusign.net/restapi/help

    public class OAuthTokenResponse {
        public string access_token;
        public string expires_in;
        public string token_type;
    }

    public class AccountCreationRequestUser {
        public string email;
        public string userName;
    }

    public class AccountCreationResponse {
        public AccountCreationResponseUser[] newUsers;
    }

    public class AccountCreationResponseUser {
        public string createdDateTime;
        public string email;
        public string uri;
        public string userId;
        public string userName;
        public string userStatus;
    }
}