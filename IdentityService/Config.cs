using System.Linq.Expressions;
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
                ClientId = "HealthMonitorApp",
                ClientName = "HealthMonitorApp",
                AllowedScopes = { "openid", "profile", "healthMonitorApp" },
                RedirectUris = { "http://localhost:3000/api/auth/callback/id-server" },
                ClientSecrets = new [] {new Secret("secret".Sha256())},
                AllowOfflineAccess = true, // Enable refresh token funcitonality
                RequirePkce = false, // This would be set to true if using for a react native mobile app
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials, // This allows access tokens to be sent internally from inside our network to Identity Server without browser involvement
                AccessTokenLifetime = 3600 * 24 * 30 // This lifetime has been extended for DEVELOPMENT PURPOSES ONLY, Default is only 3600 (1 hour)
            },
        };
}
