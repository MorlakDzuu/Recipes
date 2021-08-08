using Application.Dto;
using Application.Service;
using Domain.Recipe;
using Infastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Extranet.Api.Controllers
{
    public class RecipeController : Controller
    {
        private readonly IRecipeService _recipeService;
        private readonly ITagService _tagService;
        private readonly IUnitOfWork _unitOfWork;

        public RecipeController( IRecipeService recipeService, ITagService tagService, IUnitOfWork unitOfWork )
        {
            _recipeService = recipeService;
            _tagService = tagService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task AddRecipe( [FromBody] RecipeDto recipeDto )
        {
            //TODO: add user id
            int userId = 1;

            Recipe recipe = await _recipeService.AddRecipeAsync( recipeDto, userId );
            await _unitOfWork.Commit();

            await _tagService.AddNewTagsAsync( recipeDto.Tags );
            await _unitOfWork.Commit();

            await _tagService.AddTagsToRecipeAsync( recipeDto.Tags, recipe.Id );
            await _unitOfWork.Commit();
        }

        [HttpGet]
        public async Task<List<RecipePageDto>> GetAll()
        {
            return await _recipeService.GetAllAsync();
        }

        [HttpGet]
        [Route( "Recipe/Get/{recipeId}" )]
        public async Task<RecipePageDto> Get( int recipeId )
        {
            //TODO: add user id
            int userId = 1;

            return await _recipeService.GetRecipeById( recipeId, userId );
        }

        [HttpGet]
        [Route( "Recipe/AddLike/{recipeId}" )]
        public async Task AddLike( int recipeId )
        {
            //TODO: add user id
            int userId = 1;

            await _recipeService.AddLikeRecipeByUserAsync( userId, recipeId );
            await _unitOfWork.Commit();
        }

        [HttpGet]
        [Route( "Recipe/AddFavorite/{recipeId}" )]
        public async Task AddFavorite( int recipeId )
        {
            //TODO: add user id
            int userId = 1;

            await _recipeService.AddFavoriteRecipeByUser( userId, recipeId );
            await _unitOfWork.Commit();
        }
    }
}
