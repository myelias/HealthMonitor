using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("healthMonitorApp", "Health Monitor App Full Access")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { "scope1" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "postman",
                ClientName = "Postman",
                AllowedScopes = { "openid", "profile", "healthMonitorApp" },
                RedirectUris = { "https://www.getpostman.com/oauth2/callback" },
                ClientSecrets = new [] {new Secret("NotASecret".Sha256())},
                AllowedGrantTypes = {GrantType.ResourceOwnerPassword}
            },
            new Client
            {
                ClientId = "239824",
                ClientName = "Fitbit",
                AllowedScopes = { "openid", "profile", "healthMonitorApp" },
                RedirectUris = { "https://localhost:5000/signin-fitbit" },
                ClientSecrets = new [] {new Secret("78ca013aa8147e78910d27f91328f560".Sha256())},
                AllowedGrantTypes = {GrantType.AuthorizationCode}
            },
        };
}
