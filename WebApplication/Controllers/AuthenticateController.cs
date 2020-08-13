using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Entities;
using WebApplication.Models.User;

namespace WebApplication.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        public RoleManager<UserRole> UserRole { get; }
        public UserManager<User> UserManager { get; }

        public AuthenticateController(
            IConfiguration configuration,
            UserManager<User> user,
            RoleManager<UserRole> role
            )
        {
            Configuration = configuration;
            UserManager = user;
            UserRole = role;
        }
        [HttpPost("register", Name = nameof(CreateUserAsync))]
        public async Task<IActionResult> CreateUserAsync(RegisterUser user, string role = "guest")
        {
            var newUser = new User
            {
                UserName = user.NickName,
                PhoneNumber = user.PhoneNumber,
            };

            IdentityResult result = await UserManager.CreateAsync(newUser, user.Password);
            if (result.Succeeded)
            {
                await AddUserToRoleAsync(newUser, role);
                return Ok();
            }
            ModelState.AddModelError("Error", result.Errors.FirstOrDefault()?.Description);
            return BadRequest(ModelState);
        }

        [HttpPost("token", Name = nameof(GenerateTokenAsync))]
        public async Task<IActionResult> GenerateTokenAsync(LoginUser loginUser)
        {
            var user = await UserManager.FindByNameAsync(loginUser.UserName);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = UserManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUser.Password);
            if (result != PasswordVerificationResult.Success)
            {
                return Unauthorized();
            }
            var userClaims = await UserManager.GetClaimsAsync(user);
            var userRoles = await UserManager.GetRolesAsync(user);
            foreach (var roleItem in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, roleItem));
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            claims.AddRange(userClaims);

            var tokenConfigSection = Configuration.GetSection("Security:Token");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigSection["Key"]));
            var signCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: tokenConfigSection["Issuer"],
                audience: tokenConfigSection["audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(3),
                signingCredentials: signCredential
                );

            return Ok(new 
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                expiration = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo, TimeZoneInfo.Local)
            });
        }

        private async Task AddUserToRoleAsync(User user, string roleName)
        {
            if (user == null || string.IsNullOrWhiteSpace(roleName))
            {
                return;
            }

            bool isRoleExist = await UserRole.RoleExistsAsync(roleName);
            if (!isRoleExist)
            {
                await UserRole.CreateAsync(new UserRole { Name = roleName });
            }
            if (await UserManager.IsInRoleAsync(user, roleName))
            {
                return;
            }
            await UserManager.AddToRoleAsync(user, roleName);
        }

    }
}
