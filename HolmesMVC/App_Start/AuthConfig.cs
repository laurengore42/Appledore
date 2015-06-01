﻿namespace HolmesMVC
{
    using WebMatrix.WebData;

    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // HDB now, not AUDB
            WebSecurity.InitializeDatabaseConnection("HolmesDBEntities", "UserProfile", "UserId", "UserName", autoCreateTables: true);

            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
