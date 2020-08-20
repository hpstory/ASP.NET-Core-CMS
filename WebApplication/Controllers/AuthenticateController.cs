using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Entities;
using WebApplication.Entities.Identity.Entities;
using WebApplication.Models.User;

namespace WebApplication.Controllers
{
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public IConfiguration Configuration { get; }
        

        public AuthenticateController(
            IConfiguration configuration,
            SignInManager<User> signInManager,
            UserManager<User> userManager
            )
        {
            Configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("account/login")]
        public async Task<IActionResult> Login(LoginUser login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.PhoneNumber, login.Password, true, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(login.PhoneNumber);
                await GenerateTokenAsync(user);

            }
            return BadRequest();
        }

        [HttpPost("account/signup")]
        public async Task<IActionResult> Register(RegisterUser registerUser)
        {
            var userInfo = new User 
            { 
                NickName = registerUser.NickName, 
                UserName = registerUser.PhoneNumber,
                PhoneNumber = registerUser.PhoneNumber,
                SecurityStamp = "FS"
            };
            var result = await _userManager.CreateAsync(userInfo, registerUser.Password);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(registerUser.PhoneNumber);
                await GenerateTokenAsync(user);
            }
            return BadRequest();
        }

        public async Task<IActionResult> GenerateTokenAsync(User userModel)
        {
            if (userModel == null)
            {
                return Unauthorized();
            }
            var result = await _userManager.PasswordHasher.VerifyHashedPassword(userModel, userModel.PasswordHash, password);
            if (result != PasswordVerificationResult.Success)
            {
                return Unauthorized();
            }
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, userModel.UserName),
                new Claim(ClaimTypes.NameIdentifier, userModel.Id)
            };
            var tokenConfigSection = Configuration.GetSection("Security:Token");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigSection["Key"]));
            var signCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                issuer: tokenConfigSection["Issuer"],
                audience: tokenConfigSection["audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(30),
                signingCredentials: signCredential
                );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                expiration = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo, TimeZoneInfo.Local)
            });
        }
        //[HttpPost("register", Name = nameof(CreateUserAsync))]
        //public async Task<IActionResult> CreateUserAsync(RegisterUser user, [FromQuery] string role = "guest")
        //{
        //    var newUser = new User
        //    {
        //        UserName = user.NickName,
        //        PhoneNumber = user.PhoneNumber,
        //    };

        //    IdentityResult result = await UserManager.CreateAsync(newUser, user.Password);
        //    if (result.Succeeded)
        //    {
        //        await AddUserToRoleAsync(newUser, role);
        //        return Ok();
        //    }
        //    ModelState.AddModelError("Error", result.Errors.FirstOrDefault()?.Description);
        //    return BadRequest(ModelState);
        //}

        //[HttpPost("token", Name = nameof(GenerateTokenAsync))]
        //public async Task<IActionResult> GenerateTokenAsync(LoginUser loginUser)
        //{
        //    var user = await UserManager.FindByNameAsync(loginUser.UserName);
        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }
        //    var result = UserManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUser.Password);
        //    if (result != PasswordVerificationResult.Success)
        //    {
        //        return Unauthorized();
        //    }
        //    var userClaims = await UserManager.GetClaimsAsync(user);
        //    var userRoles = await UserManager.GetRolesAsync(user);
        //    foreach (var roleItem in userRoles)
        //    {
        //        userClaims.Add(new Claim(ClaimTypes.Role, roleItem));
        //    }

        //    var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //    };
        //    claims.AddRange(userClaims);

        //    var tokenConfigSection = Configuration.GetSection("Security:Token");
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigSection["Key"]));
        //    var signCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var jwtToken = new JwtSecurityToken(
        //        issuer: tokenConfigSection["Issuer"],
        //        audience: tokenConfigSection["audience"],
        //        claims: claims,
        //        expires: DateTime.Now.AddMinutes(3),
        //        signingCredentials: signCredential
        //        );

        //    return Ok(new 
        //    {
        //        token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
        //        expiration = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo, TimeZoneInfo.Local)
        //    });
        //}

        //private async Task AddUserToRoleAsync(User user, string roleName)
        //{
        //    if (user == null || string.IsNullOrWhiteSpace(roleName))
        //    {
        //        return;
        //    }

        //    bool isRoleExist = await UserRole.RoleExistsAsync(roleName);
        //    if (!isRoleExist)
        //    {
        //        await UserRole.CreateAsync(new UserRole { Name = roleName });
        //    }
        //    if (await UserManager.IsInRoleAsync(user, roleName))
        //    {
        //        return;
        //    }
        //    await UserManager.AddToRoleAsync(user, roleName);
        //}

    }
}
