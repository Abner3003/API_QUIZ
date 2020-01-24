using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Quiz.Aplicacao.API.Dtos;
using Quiz.Dominio.API.Modelos;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Quiz.Aplicacao.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
    //    private readonly TokenConfigurations _tokenConfigurations;
    //    private readonly SigningConfigurations _signingConfigurations;
    //    public LoginController(TokenConfigurations tokenConfigurations, SigningConfigurations signingConfigurations)
    //    {
    //        _tokenConfigurations = tokenConfigurations;
    //        _signingConfigurations = signingConfigurations;
    //    }


    //    [AllowAnonymous]
    //    [HttpPost]
    //    public object Authenticate([FromBody]UserModel user)
    //    {
    //        bool credenciaisValidas = true;

    //        var usuarios = new List<UserModel>();
    //        var usuarionovo  = new UserModel();

    //        usuarios.Add(usuarionovo);

    //        var usuarioList = new List<User>();
    //        var usuarioTeste = new User();

    //        usuarioTeste.UserID = "abnec3003@gmail.com";
    //        usuarioTeste.AccessKey = "123456";

    //        usuarioList.Add(usuarioTeste);


    //        if (credenciaisValidas)
    //        {
    //            ClaimsIdentity identity = new ClaimsIdentity(
    //                new GenericIdentity(user.UserID, "Login"),
    //                new[] {
    //                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
    //                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserID)
    //                }
    //            );

    //            DateTime dataCriacao = DateTime.Now;
    //            DateTime dataExpiracao = dataCriacao +
    //            TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

    //            var handler = new JwtSecurityTokenHandler();

    //            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
    //            {
    //                Issuer = _tokenConfigurations.Issuer,
    //                Audience = _tokenConfigurations.Audience,
    //                SigningCredentials = _signingConfigurations.SigningCredentials,
    //                Subject = identity,
    //                NotBefore = dataCriacao,
    //                Expires = dataExpiracao
    //            });
    //            var token = handler.WriteToken(securityToken);
    //            return new
    //            {
    //                authenticated = true,
    //                created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
    //                expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
    //                accessToken = token,
    //                message = "OK",
    //                name = "usuarioTeste"

    //            };


    //        }
    //        else
    //        {
    //            return new
    //            {
    //                authenticated = false,
    //                message = "Falha ao autenticar"
    //            };
    //        }


    //    }
    //}
}}