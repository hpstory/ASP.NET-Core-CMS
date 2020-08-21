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
                if (user == null)
                {
                    return Unauthorized();
                }
                var loginResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, login.Password);
                if (loginResult != PasswordVerificationResult.Success)
                {
                    return Unauthorized();
                }
                if (login.Remenber)
                {
                    GenerateTokenAsync(user, LoginState.NoState);
                }
                return Ok();
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
            };
            if (string.IsNullOrEmpty(registerUser.Captcha))
            {
                return BadRequest("验证码为空");
            }
            if (registerUser.Captcha != HttpContext.Session.GetString("LoginValidateCode"))
            {
                return BadRequest("验证码错误");
            }
            var result = await _userManager.CreateAsync(userInfo, registerUser.Password);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(registerUser.PhoneNumber);
                GenerateTokenAsync(user);
            }
            ModelState.AddModelError("Error", result.Errors.FirstOrDefault()?.Description);
            return BadRequest("注册失败");
        }
        [Route("token")]
        public IActionResult GenerateTokenAsync(User user, LoginState state = LoginState.login)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var tokenConfigSection = Configuration.GetSection("Security:Token");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigSection["Key"]));
            var signCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            if (state == LoginState.login)
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
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    expiration = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo, TimeZoneInfo.Local)
                });
            }

        }
        [Route("getCaptcha")]
        public IActionResult GetCaptchaImage()
        {
            Tuple<string, string> captchaCode = CaptchaHelper.GetCaptchaCode();
            byte[] bytes = CaptchaHelper.CreateCaptchaImage(captchaCode.Item1);
            HttpContext.Session.SetString("LoginValidateCode", captchaCode.Item2);
            return File(bytes, @"image/jpeg");
        }
    }
}
