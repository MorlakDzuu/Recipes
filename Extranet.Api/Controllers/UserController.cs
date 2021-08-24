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
using Application.Dto.User;
using System.Linq;
using System.Security.Claims;
using Domain.User;

namespace Extranet.Api.Controllers
{
    [Route( "[controller]" )]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UserController(
            IUserService userService,
            IPasswordService passwordService,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository )
        {
            _userService = userService;
            _passwordService = passwordService;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        [Authorize( AuthenticationSchemes = "Bearer" )]
        [HttpGet, Route( "info" )]
        public async Task<IActionResult> GetUserProfileInfo()
        {
            int userId = int.Parse( User.Claims.First( c => c.Type == ClaimTypes.NameIdentifier ).Value );

            try
            {
                UserProfileDto userProfileDto = await _userService.GetUserProfileInfoAsync( userId );
                return Ok( userProfileDto );
            }
            catch ( Exception )
            {
                return BadRequest();
            }
        }

        [Authorize( AuthenticationSchemes = "Bearer" )]
        [HttpPost, Route( "edit" )]
        public async Task<IActionResult> Edit( [FromBody] UserEditDto userEditDto )
        {
            int userId = int.Parse( User.Claims.First( c => c.Type == ClaimTypes.NameIdentifier ).Value );

            try
            {
                User user = await _userRepository.GetAsync( userId );
                user.Name = userEditDto.Name;
                user.Login = userEditDto.Login;
                user.Description = userEditDto.Description;
                await _unitOfWork.Commit();

                return Ok();
            } catch(Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost, Route( "register" )]
        public async Task<IActionResult> Register( [FromBody] UserRegistrationDto userRegistrationDto )
        {
            try
            {
                userRegistrationDto.Password = _passwordService.HashPassword( userRegistrationDto.Password, RandomNumberGenerator.Create() );

                await _userService.AddAsync( userRegistrationDto );
                await _unitOfWork.Commit();
            }
            catch ( Exception )
            {
                return BadRequest( new { message = "Не получилось зарегестрировать пользователя" } );
            }

            return Ok();
        }

        [HttpPost, Route( "login" )]
        public async Task<IActionResult> Login( [FromBody] UserLoginDto userLoginDto )
        {
            var identity = await _passwordService.GetIdentityAsync( userLoginDto.Login, userLoginDto.Password );

            if ( identity == null )
            {
                return BadRequest( new { message = "Неправильное имя пользователя или пароль." } );
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

            UserDto userDto = await _userService.GetByLoginAsync( userLoginDto.Login );

            var userSettings = new
            {
                name = userDto.Name,
                login = userDto.Login,
                token = encodedJwt
            };

            return Ok( userSettings );
        }

        [HttpGet, Route( "logout" )]
        [Authorize( AuthenticationSchemes = "Bearer" )]
        public IActionResult Logoff()
        {
            try
            {
                HttpContext.Session.Clear();
                return Ok();
            }
            catch ( Exception )
            {
                return BadRequest();
            }
        }

        [HttpGet, Route( "verify" )]
        [Authorize( AuthenticationSchemes = "Bearer" )]
        public IActionResult VerifyToken()
        {
            return Ok( "token работает" );
        }
    }
}
