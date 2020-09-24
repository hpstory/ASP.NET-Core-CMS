using Microsoft.AspNetCore.Authorization;


namespace Blog.IdentityServer.Auths
{
    public class ClaimRequirement : IAuthorizationRequirement
    {
        public ClaimRequirement(string claimValue)
        {
            ClaimValue = claimValue;
        }

        public string ClaimValue { get; set; }
    }
}
