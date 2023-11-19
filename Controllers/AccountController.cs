using Azure;
using HotelServer.Common;
using HotelServer.Controllers.request;
using HotelServer.Data;
using HotelServer.Model;
using HotelServer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelServer.Controllers
{
    public interface IAccountController
    {
        public Task<IActionResult> Register(RegistrationRequest request);
        public Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request);
        public Task<IActionResult> ChangePassword(ChangePasswordRequest request);
        public Task<IActionResult> ForgotPassword(ForgotPasswordRequest request);
        public Task<IActionResult> ResetPassword(ResetPasswordRequest request);
        public Task<IActionResult> GetInfor(string i);
    }

    [Route("[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class AccountController : ControllerBase, IAccountController
    {
        private readonly UserManager<User> _userManager;
        private readonly HotelDbContext _context;
        private readonly IConfigurationRoot configuration;
        private readonly IMailService _mailService;

        public AccountController(UserManager<User> userManager, HotelDbContext context, IMailService mailService)
        {
            _userManager = userManager;
            _context = context;
            configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
            _mailService = mailService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(RegistrationRequest request)
        {
            var response = new AuthResponse();
            if (!ModelState.IsValid)
            {
                response.State = false;
                response.Message = "A error has occurred!";
                return BadRequest(response);
            }

            var result = await _userManager.CreateAsync(
            new User { UserName = request.Username, Email = request.Email, Name = request.Name, Role = "user" },
            request.Password
            );
            if (!result.Succeeded)
            {
                response.State = false;
                response.Message = "Fail to create account";
                return BadRequest(response);
            }

            response.State = true;
            response.Message = "Success created account";
            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
        {
            var response = new AuthResponse();

            if (!ModelState.IsValid)
            {
                response.State = false;
                response.Message = "A error have occured";
                return BadRequest(response);
            }

            //check account is exist
            var managedUser = await _userManager.FindByNameAsync(request.Username);
            if (managedUser == null)
            {
                response.State = false;
                response.Message = "Account does not exist!";
                return BadRequest(response);
            }

            //check password
            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
            if (!isPasswordValid)
            {
                response.State = false;
                response.Message = "Password is not correct!";
                return BadRequest(response);
            }

            //get user data in database
            var userInDb = _context.Users.FirstOrDefault(u => u.UserName == request.Username);
            if (userInDb is null)
                return Unauthorized();
            

            await _context.SaveChangesAsync();
            return Ok(new AuthResponse
            {
                State = true,
                Message = GeneralToken(userInDb),
                Data = new {Role = userInDb.Role, Id = userInDb.Id, Name = userInDb.Name}
            });
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var response = new AuthResponse();

            //check account is exist
            var managedUser = await _userManager.FindByNameAsync(request.Username);
            if (managedUser == null)
            {
                response.State = false;
                response.Message = "Account does not exist!";
                return BadRequest(response);
            }

            //check old password
            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.OldPassword);
            if (!isPasswordValid)
            {
                response.State = false;
                response.Message = "Old Password is not correct!";
                return BadRequest(response);
            }

            var result = await _userManager.ChangePasswordAsync(managedUser, request.OldPassword, request.Password);
            if(!result.Succeeded)
            {
                response.State = false;
                response.Message = "A error have occurred!";
                return BadRequest(response);
            }

            response.State = true;
            response.Message = "Change password successful!";

            return Ok(response);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var response = new AuthResponse();

            if (!ModelState.IsValid)
            {
                response.State = false;
                response.Message = "A error have occured";
                return BadRequest(response);
            }

            //check email is exist
            var managedUser = await _userManager.FindByEmailAsync(request.Email);
            if (managedUser == null)
            {
                response.State = false;
                response.Message = "Email does not exist!";
                return BadRequest(response);
            }

            //token
            var token = await _userManager.GeneratePasswordResetTokenAsync(managedUser);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            //url
            string url = $"{configuration["AppUrl"]}/ResetPassword?email={managedUser.Email}&token={validToken}";

            //sending email
            await _mailService.SendEmailAsync(managedUser.Email, "Reset Password", "<h1>Follow the instructions to reset your password</h1>"
                + $"<p> To reset your password: <a href='{url}'>Click here</a></p>");

            response.State = true;
            response.Message = "The email for reset password has been sent!";

            return Ok(response);
        }
        
        
        [HttpPut]
        [AllowAnonymous]
        [Route("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var response = new AuthResponse();

            if (!ModelState.IsValid)
            {
                response.State = false;
                response.Message = "A error has occurred!";
                return BadRequest(response);
            }

            //check email is exist
            var managedUser = await _userManager.FindByEmailAsync(request.Email);
            if (managedUser == null)
            {
                response.State = false;
                response.Message = "Email does not exist!";
                return BadRequest(response);
            }

            var decodeToken = WebEncoders.Base64UrlDecode(request.Token);
            string normalToken = Encoding.UTF8.GetString(decodeToken);

            var result = await _userManager.ResetPasswordAsync(managedUser, normalToken, request.NewPassword);
            if(result.Succeeded)
            {
                response.State = true;
                response.Message = "Reset password successful!";
            }
            else
            {
                response.State = false;
                response.Message = "Reset password failure!";
                return BadRequest(response);
            }

            return Ok(response);
        }
        
        //[HttpPost]
        //[AllowAnonymous]
        //[Route("verifi")]
        //public async Task<IActionResult> VerifyAccount(VerifyAccountRequest request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}
        
        private string GeneralToken(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:IssuerSigningKey"]));

            var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.UtcNow.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet]
        [Authorize]
        [Route("getInfor")]
        public async Task<IActionResult> GetInfor([FromQuery]string id)
        {
            var response = new AuthResponse();
            if(id == null)
            {
                response.State = false;
                response.Message = "Id must not null";
                return BadRequest(response);
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                response.State = false;
                response.Message = "User is not exist!";
                return BadRequest(response);
            }

            user.PasswordHash = "";

            response.State = true;
            response.Message = "Get infor successful!";
            response.Data = user;
            return Ok(response);
        }
        
        
        [HttpPut]
        [Authorize]
        [Route("updateInfor")]
        public async Task<IActionResult> ChangeInfor(ChangeInforRequest request)
        {
            var response = new AuthResponse();
            //get user from db
            var userdb = await _userManager.FindByIdAsync(request.Id);
            if (userdb == null)
            {
                response.State = false;
                response.Message = "Account does not exist!";
                return BadRequest(response);
            }

            //validate
            if(request.Email == string.Empty)
            {
                response.State = false;
                response.Message = "missing fields required";
                return BadRequest(response);
            }

            userdb.Email = request.Email;
            userdb.PhoneNumber = request.PhoneNumber;
            userdb.Address = request.address;

            var result = await _userManager.UpdateAsync(userdb);
            if (!result.Succeeded)
            {
                response.State = false;
                response.Message = result.Errors.First().Description;
                return BadRequest(response);
            }

            response.State = true;
            response.Message = "Update information successful!";
            return Ok(response);
        }

    }
}
