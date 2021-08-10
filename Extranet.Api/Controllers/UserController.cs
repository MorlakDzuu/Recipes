using Application.Dto;
using Application.Service;
using Domain.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Extranet.Api.Auth;
using Microsoft.AspNetCore.Http;
using Infastructure;
using Microsoft.AspNetCore.Authorization;

namespace Extranet.Api.Controllers
{
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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task RegisterUser( [FromBody] UserRegistrationDto userRegistrationDto )
        {
            await _userService.AddAsync( userRegistrationDto );
            await _unitOfWork.Commit();
        }

        public async Task<List<UserDto>> GetAll()
        {
            return await _userService.GetAllAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Login( string login, string password )
        {
            var identity = await _passwordService.GetIdentityAsync( login, password );

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

        [Authorize( AuthenticationSchemes = "Bearer" )]
        [HttpGet]
        public IActionResult Logoff()
        {
            HttpContext.Session.Clear();
            return Ok();
        }
    }
}
