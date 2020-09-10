using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Entities.Enum;
using WebApplication.Entities.Identity.Entities;
using WebApplication.Helpers;
using WebApplication.Infrastructure;
using WebApplication.Models.UserInfo;

namespace WebApplication.Controllers
{
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IRepositoryWrapper RepositoryWrapper { get; }
        private IMapper Mapper { get; }
        private IConfiguration Configuration { get; }
        

        public AuthenticateController(
            IConfiguration configuration,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<IdentityRole>roleManager,
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper
            )
        {
            RepositoryWrapper = repositoryWrapper;
            Configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            Mapper = mapper;
            _roleManager = roleManager;
        }

        [HttpPost("account/login")]
        public async Task<IActionResult> Login(LoginUser login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.PhoneNumber, login.Password, true, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(login.PhoneNumber);
                if (user == null)
                {
                    return Unauthorized();
                }
                var loginResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, login.Password);
                if (loginResult != PasswordVerificationResult.Success)
                {
                    return Unauthorized();
                }
                if (!login.Remember)
                {
                    return GenerateTokenAsync(user, LoginState.NoState);
                }
                return GenerateTokenAsync(user, LoginState.Login);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("account/signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUser registerUser)
        {
            User isRegist = await _userManager.FindByNameAsync(registerUser.NickName);
            if (isRegist != null)
            {
                return BadRequest(new { description = "用户名已被注册！" });
            }
            var userInfo = new User 
            { 
                NickName = registerUser.NickName, 
                UserName = registerUser.PhoneNumber,
                PhoneNumber = registerUser.PhoneNumber,
            };
            if (string.IsNullOrEmpty(registerUser.Captcha))
            {
                return BadRequest(new { description = "验证码为空！" });
            }
            if (registerUser.Captcha != HttpContext.Session.GetString("LoginValidateCode"))
            {
                return BadRequest(new { description = "验证码错误！" });
            }
            var result = await _userManager.CreateAsync(userInfo, registerUser.Password);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(registerUser.PhoneNumber);
                return GenerateTokenAsync(user);
            }
            // ModelState.AddModelError("Error", result.Errors.FirstOrDefault()?.Description);
            return BadRequest(new { result.Errors.FirstOrDefault()?.Description });

        }
        [Route("token")]
        [ApiExplorerSettings(IgnoreApi = true)]
        private IActionResult GenerateTokenAsync(User user, LoginState state = LoginState.Login)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.PhoneNumber),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var tokenConfigSection = Configuration.GetSection("Security:Token");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigSection["Key"]));
            var signCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var userInfo = Mapper.Map<UserInfoDto>(user);
            if (state == LoginState.Login)
            {
                var jwtToken = new JwtSecurityToken(
                    issuer: tokenConfigSection["Issuer"],
                    audience: tokenConfigSection["audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(30),
                    signingCredentials: signCredential
                );
                return Ok(new
                {
                    userInfo,
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    expiration = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo, TimeZoneInfo.Local)
                });
            }
            else
            {
                var jwtToken = new JwtSecurityToken(
                    issuer: tokenConfigSection["Issuer"],
                    audience: tokenConfigSection["audience"],
                    claims: claims,
                    expires: DateTime.Now,
                    signingCredentials: signCredential
                );
                return Ok(new
                {
                    userInfo,
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    expiration = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo, TimeZoneInfo.Local)
                });
            }

        }

        [HttpPut("account/editProfile")]
        [HttpCacheExpiration]
        [HttpCacheValidation]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> UpdateUserInfoAsync(UserInfoAddDto userInfo)
        {
            if (userInfo == null)
            {
                throw new ArgumentNullException(nameof(userInfo));
            }
            var result = await _userManager.FindByNameAsync(userInfo.UserName);
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }
            result.NickName = userInfo.NickName;
            result.Avatar = userInfo.Avatar;
            result.BirthDate = userInfo.BirthDate;
            result.City = userInfo.City;
            result.Province = userInfo.Province;
            RepositoryWrapper.Users.Update(result);
            await RepositoryWrapper.Users.SaveAsync();
            return NoContent();
        }
        [HttpGet("role")]
        public IActionResult GetRolesAsync()
        {
            var result = _roleManager.Roles.ToList();
            return Ok(result);
        }

        [HttpPost("createRole")]
        public async Task<ActionResult> CreateRoleAsync(string role, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(role));
            if (roleResult.Succeeded)
            {
                var userResult = await _userManager.AddToRoleAsync(user, role);
                if (userResult.Succeeded)
                {
                    return CreatedAtRoute(nameof(GetRolesAsync), role);
                }
                return BadRequest(userResult.Errors);
            }
            return BadRequest(roleResult.Errors);
           
        }

        [HttpGet("getCaptcha")]
        [HttpCacheExpiration(NoStore = true)]
        public IActionResult GetCaptchaImage()
        {
            Tuple<string, string> captchaCode = CaptchaHelper.GetCaptchaCode();
            byte[] bytes = CaptchaHelper.CreateCaptchaImage(captchaCode.Item1);
            HttpContext.Session.SetString("LoginValidateCode", captchaCode.Item2);
            return File(bytes, @"image/jpeg");
        }
    }
}
