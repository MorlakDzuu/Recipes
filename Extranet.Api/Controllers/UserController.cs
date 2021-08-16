using Application.Dto;
using Application.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Extranet.Api.Auth;
using Microsoft.AspNetCore.Http;
using Infastructure;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

namespace Extranet.Api.Controllers
{
    [Route( "[controller]" )]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly IUnitOfWork _unitOfWork;

        public UserController( IUserService userService, IPasswordService passwordService, IUnitOfWork unitOfWork )
        {
            _userService = userService;
            _passwordService = passwordService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost, Route( "register" )]
        public async Task Register( [FromBody] UserRegistrationDto userRegistrationDto )
        {
            userRegistrationDto.Password = _passwordService.HashPassword( userRegistrationDto.Password, RandomNumberGenerator.Create() );

            await _userService.AddAsync( userRegistrationDto );
            await _unitOfWork.Commit();
        }

        [HttpPost, Route( "login" )]
        public async Task<IActionResult> Login( [FromBody] UserLoginDto userLoginDto )
        {
            var identity = await _passwordService.GetIdentityAsync( userLoginDto.Login, userLoginDto.Password );

            if ( identity == null )
            {
                return BadRequest( new { errorText = "Неправильное имя пользователя или пароль." } );
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add( TimeSpan.FromMinutes( AuthOptions.LIFETIME ) ),
                    signingCredentials: new SigningCredentials( AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256 ) );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken( jwt );

            HttpContext.Session.SetString( "JWToken", encodedJwt );
            var response = new
            {
                access_token = encodedJwt
            };

            return Json( response );
        }

        [HttpGet, Route("logout")]
        [Authorize( AuthenticationSchemes = "Bearer" )]
        public IActionResult Logoff()
        {
            HttpContext.Session.Clear();
            return Ok();
        }
    }
}
