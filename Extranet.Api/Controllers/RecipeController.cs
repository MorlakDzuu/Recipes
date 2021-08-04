using Application.Dto;
using Application.Service;
using Domain.Recipe;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Extranet.Api.Controllers
{
    public class RecipeController : Controller
    {
        private IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task AddRecipe([FromBody] RecipeDto recipeDto)
        {
            //TODO: add user id
            int userId = 1;

            await _recipeService.AddRecipeAsync(recipeDto, userId);
        }

        [HttpGet]
        public async Task<List<Recipe>> GetAll()
        {
            return await _recipeService.GetAllAsync();
        }

        [HttpGet]
        [Route("Recipe/Get/{recipeId}")]
        public async Task<RecipePageDto> Get( int recipeId )
        {
            //TODO: add user id
            int userId = 1;

            return await _recipeService.GetRecipeById( recipeId, userId );
        }

        [HttpGet]
        [Route("Recipe/AddLike/{recipeId}")]
        public async Task AddLike( int recipeId )
        {
            //TODO: add user id
            int userId = 1;

            await _recipeService.AddLikeRecipeByUserAsync( userId, recipeId );
        }

        [HttpGet]
        [Route("Recipe/AddFavorite/{recipeId}")]
        public async Task AddFavorite( int recipeId )
        {
            //TODO: add user id
            int userId = 1;

            await _recipeService.AddFavoriteRecipeByUser( userId, recipeId );
        }
    }
}
