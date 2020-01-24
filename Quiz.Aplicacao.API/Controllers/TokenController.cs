using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Quiz.Aplicacao.API.Dtos;
using Quiz.Dominio.API.Modelos;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Quiz.Aplicacao.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _config;
        private TokenConfigurations _tokenConfigurations;

        public TokenController(IConfiguration configuration, TokenConfigurations tokenConfigurations)
        {
            _config = configuration;
            _tokenConfigurations = tokenConfigurations;

        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = Authenticate(login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }


        private object BuildToken(UserModel user)
        {

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Birthdate, user.Birthdate.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.WriteToken(token);
            return new
            {
                authenticated = true,
                created = DateTime.Now,
                expiration = DateTime.Now + TimeSpan.FromSeconds(_tokenConfigurations.Seconds),
                acessToken = securityToken,
                message = "OK"
            };
        }

        private UserModel Authenticate(LoginModel login)
        {
            UserModel user = null;

            if (login.Username == "mario" && login.Password == "secret")
            {
                user = new UserModel { Name = "Mario Rossi", Email = "mario.rossi@domain.com" };
            }
            return user;
        }

    }
}