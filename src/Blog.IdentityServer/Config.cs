// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Blog.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles", "role", new List<string>{ JwtClaimTypes.Role }),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("blog.api"),
                new ApiScope("blog.cms"),
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource
                {
                    Name = "frontend.api",
                    Scopes = { "blog.api" },
                    ApiSecrets = { new Secret("0ed8ca6e-6dcf-4d46-b38f-dfc784e362cd".Sha256()) }
                },
                new ApiResource
                {
                    Name = "backend.api",
                    Scopes = { "blog.cms" },
                    ApiSecrets = { new Secret("0ed8ca6e-6dcf-4d46-b38f-dfc784e362cd".Sha256()) }
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // blog前端
                new Client
                {
                    ClientId = "angular-front-client",
                    ClientName = "Angular Frontend",
                    ClientUri = "http://localhost:4200",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = true,
                    AccessTokenLifetime = 60 * 5,
                    RedirectUris =
                    {
                        "http://localhost:4200/signin-oidc",
                        "http://localhost:4200/redirect-silent-renew"
                    },
                    FrontChannelLogoutUri = "http://localhost:4200",
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:4200/"
                    },
                    AllowedCorsOrigins =
                    {
                        "http://localhost:4200"
                    },

                    AllowedScopes = 
                    {
                        "blog.api",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    }
                },

                new Client
                {
                    ClientId = "angular-cms-client",
                    ClientName = "Angular Backend",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                    AllowAccessTokensViaBrowser = true,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireConsent = true,
                    AccessTokenLifetime = 60 * 5,
                    RedirectUris =
                    {
                        "https://localhost:4201/signin-oidc",
                        "http://localhost:4201/redirect-silent-renew"
                    },
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:4201/"
                    },
                    AllowedCorsOrigins =
                    {
                        "http://localhost:4201"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "blog.cms",
                        "roles"
                    }
                },
            };
    }
}