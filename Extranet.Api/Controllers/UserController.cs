using Application.Dto;
using Application.Service;
using Domain.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Extranet.Api.Controllers
{
    public class UserController : Controller
    {
        private IUserService _userService;

        public UserController( IUserService userService )
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task AddUser()
        {
            await _userService.AddAsync( "Ivan", "ivan228", "some description", "123" );
        }

        public async Task<List<UserDto>> GetAll()
        {
            return await _userService.GetAllAsync();
        }
    }
}
