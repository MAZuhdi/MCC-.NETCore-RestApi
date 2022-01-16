using API.Models;
using API.Repository;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace API.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository accountRepository;
        public IConfiguration _configuration;
        public AccountsController(AccountRepository accountRepository, IConfiguration configuration) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
            this._configuration = configuration;
        }

        [HttpPost("Login")]
        public ActionResult Login(LoginVM loginVM)
        {
            var login = accountRepository.Login(loginVM.Email, loginVM.Password);

            if (login == 2)
            {
                return Unauthorized(new JWTokenVM{ status = login.ToString(), idToken = null, message = "Akun tidak ditemukan" });
            }
            if (login == 3)
            {
                return Unauthorized(new JWTokenVM{ status = login.ToString(), idToken = null, message = "Password salah" });
            }
            if (login == 1)
            {
                var tokenPayload = accountRepository.TokenPayload(loginVM.Email);

                var claims = new List<Claim>
                {
                    new Claim("email", loginVM.Email),
                };

                foreach (var role in tokenPayload.Roles)
                {
                    claims.Add(new Claim("roles", role));
                }
                
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //sebagai Header
                var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(10),
                            signingCredentials: signIn
                            );
                var idtoken = new JwtSecurityTokenHandler().WriteToken(token); //Generate token
                claims.Add(new Claim("TokenSecurity", idtoken.ToString()));
                return Ok(new JWTokenVM{ status = "success", idToken = idtoken, message = "Successfuly logged in" });
            }
            else
            {
                return BadRequest(new { status = "failed", message = "Unknown error" });
            }
        }

        [HttpGet("ForgotPassword/{email}")]
        public ActionResult ForgotPassword(string email)
        {
            int ForgotPassword = accountRepository.ForgotPassword(email);

            if (ForgotPassword == 2)
            {
                return NotFound(new { status = "failed", message = "Email not found" });
            }            
            if (ForgotPassword == 3)
            {
                return BadRequest(new { status = "failed", message = "Unknown Error" });
            }

            return Ok(new { status = "success", message = "Email sent", otp = ForgotPassword });
        }

        [HttpPost("ChangePassword")]
        public ActionResult ChangePassword(ChangePasswordVM changePasswordVM)
        {
            //return Ok(new { nik = changePasswordVM.NIK, otp = changePasswordVM.OTP, newpassword = changePasswordVM.NewPassword });
            var change = accountRepository.ChangePassword(changePasswordVM.email, changePasswordVM.OTP, changePasswordVM.NewPassword);

            if (change == 2)
            {
                return NotFound(new { status = "failed", message = "Account not found" });
            }
            if (change == 3)
            {
                return BadRequest(new { status = "failed", message = "Invalid OTP" });
            }            
            if (change == 4)
            {
                return BadRequest(new { status = "failed", message = "OTP expired, please request new OTP" });
            }            
            if (change == 5)
            {
                return BadRequest(new { status = "failed", message = "OTP already been used, please request new OTP" });
            }            
            if (change == 6 || change == 7)
            {
                return BadRequest(new { status = "failed", message = "Update failed" });
            }
            if (change == 1)
            {
                return Ok(new { status = "success", message = "password changed" });
            }
            else
            {
                return BadRequest(new {status = "failed", message = "unknown error"});
            }
        }

        [Authorize]
        [HttpGet("TestJWT")]
        public ActionResult TestJWT()
        {
            return Ok("Test JWT Berhasil");
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("TestEmployee")]
        public ActionResult TestEmployee()
        {
            return Ok("Test JWT Employee Berhasil");
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("TestManager")]
        public ActionResult TestManager()
        {
            return Ok("Test JWT Manager Berhasil");
        }

        [Authorize(Roles = "Director")]
        [HttpGet("TestDirector")]
        public ActionResult TestDirector()
        {
            return Ok("Test JWT Director Berhasil");
        }
    }
}
