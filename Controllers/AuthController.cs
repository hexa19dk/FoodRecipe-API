using FoodFestAPI.Data;
using FoodFestAPI.Models;
using FoodFestAPI.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace FoodFestAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _ctx;
        private ApiResponse _response;
        private readonly UserManager<AppUser> _userMgr;
        private readonly RoleManager<IdentityRole> _roleMgr;
        private string secretKey;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AuthController(ApplicationDbContext ctx, IConfiguration config, UserManager<AppUser> userMgr, RoleManager<IdentityRole> roleMgr, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _ctx = ctx;
            _response = new ApiResponse();
            _userMgr = userMgr;
            _roleMgr = roleMgr;
            secretKey = config.GetValue<string>("ApiSettings:Secret")!;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO req)
        {

            AppUser getUser = await _ctx.AppUsers.FirstOrDefaultAsync(u => u.UserName.ToLower() == req.Email);

            if (getUser != null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists");
                return BadRequest(_response);
            }

            AppUser newUser = new()
            {
                Name = req.Name,
                UserName = req.Email,
                Email = req.Email,
                NormalizedEmail = req.Email.ToUpper(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            try
            {
                var result = await _userMgr.CreateAsync(newUser, req.Password);

                if (result.Succeeded)
                {
                    // Create data to Role Table
                    if (!_roleMgr.RoleExistsAsync(Models.Enum.Admin).GetAwaiter().GetResult())
                    {
                        await _roleMgr.CreateAsync(new IdentityRole(Models.Enum.Admin));
                        await _roleMgr.CreateAsync(new IdentityRole(Models.Enum.User));
                    }

                    if (req.Role.ToLower() == Models.Enum.Admin)
                    {
                        await _userMgr.AddToRoleAsync(newUser, Models.Enum.Admin);
                    }
                    else
                    {
                        await _userMgr.AddToRoleAsync(newUser, Models.Enum.User);
                    }

                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    return Ok(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string>() { ex.Message.ToString() };
            }

            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("Error occure while registering");
            return BadRequest(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO req)
        {
            AppUser getUser = await _ctx.AppUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == req.Email.ToLower());

            bool isValid = await _userMgr.CheckPasswordAsync(getUser, req.Password);
            if (isValid == false)
            {
                _response.Result = new LoginResponseDTO();
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            JwtSecurityTokenHandler tokenHandler = new();
            var roles = await _userMgr.GetRolesAsync(getUser);
            byte[] key = Encoding.ASCII.GetBytes(secretKey);

            SecurityTokenDescriptor tokenDesc = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Fullname", getUser.UserName),
                    new Claim("id", getUser.Id.ToString()),
                    new Claim(ClaimTypes.Email, getUser.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDesc);

            LoginResponseDTO resp = new()
            {
                Email = getUser.Email,
                Token = tokenHandler.WriteToken(token)
            };

            if (resp.Email == null || string.IsNullOrEmpty(resp.Token))
            {
                _response.Result = new LoginResponseDTO();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Email or token is not found !");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = resp;
            return Ok(_response);
        }

        [NonAction]
        public string MailBody(string mail, string resetLink)
        {
            return $@"
            <html>
                <body>
                    <p>Hi {mail},</p>
                    <p>We received a request to reset your password. Click the link below to reset your password:</p>
                    <p><a href='{resetLink}'>Reset Password</a></p>
                    <p>If you did not request a password reset, please ignore this email or contact support if you have questions.</p>
                    <p>Thanks,</p>
                    <p>The Team</p>
                </body>
            </html>";
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var resetPath = "http://localhost:3000/ResetPassword?" + $"token={token}&" + $"email={user.Email}";

                await _emailSender.SendEmailAsync(dto.Email, "Reset Password", MailBody(dto.Email, HtmlEncoder.Default.Encode(resetPath)));

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = new ForgotPasswordDTO();
                return Ok(_response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(resetDto.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            
            var token = WebEncoders.Base64UrlDecode(resetDto.Token);
            var result = await _userManager.ResetPasswordAsync(user, Encoding.UTF8.GetString(token), resetDto.Password);
            if (result.Succeeded)
            {
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = ModelState;
            return BadRequest(_response);
        }
    }
}
