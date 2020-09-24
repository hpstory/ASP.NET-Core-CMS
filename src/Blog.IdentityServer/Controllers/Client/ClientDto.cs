using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.IdentityServer.Controllers.Client
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientSecrets { get; set; }
        public string Description { get; set; }
        public string AllowedGrantTypes { get; set; }
        public string AllowedScopes { get; set; }
        public string AllowedCorsOrigins { get; set; }
        public string RedirectUris { get; set; }
        public string PostLogoutRedirectUris { get; set; }
    }
}
