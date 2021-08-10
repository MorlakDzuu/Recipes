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

            await _recipeService.AddFavoriteRecipeByUserAsync( userId, recipeId );
            await _unitOfWork.Commit();
        }

        [HttpGet]
        [Route( "Recipe/DeleteLike/{recipeId}" )]
        public async Task DeleteLike( int recipeId )
        {
            //TODO: add user id
            int userId = 1;

            await _recipeService.DeleteLikeByUserAsync( userId, recipeId );
            await _unitOfWork.Commit();
        }

        [HttpGet]
        [Route( "Recipe/DeleteFavorite/{recipeId}" )]
        public async Task DeleteFavorite( int recipeId )
        {
            //TODO: add user id
            int userId = 1;

            await _recipeService.DeleteFavoriteByUserAsync( userId, recipeId );
            await _unitOfWork.Commit();
        }

        [HttpPost]
        [Route( "Recipe/Edit/{recipeId}" )]
        public async Task EditRecipe( [FromBody] RecipeDto recipeDto, int recipeId )
        {
            await _recipeService.EditRecipeAsync( recipeDto, recipeId );
            await _tagService.AddNewTagsAsync( recipeDto.Tags );
            await _unitOfWork.Commit();

            await _tagService.DeleteOldTagsAsync( recipeDto.Tags, recipeId );
            await _tagService.AddTagsToRecipeAsync( recipeDto.Tags, recipeId );
            await _unitOfWork.Commit();
        }

        [HttpGet]
        [Route( "Recipe/Delete/{recipeId}" )]
        public async Task DeleteRecipe( int recipeId )
        {
            await _recipeService.DeleteRecipeByIdAsync( recipeId );
            await _unitOfWork.Commit();
        }

        [HttpGet]
        [Route( "Recipe/Feed/{pageNumber}" )]
        public async Task<List<RecipeFeedDto>> GetFeed( int pageNumber )
        {
            //TODO: add userId
            int userId = 1;

            return await _recipeService.GetRecipesFeedAsync( pageNumber, userId );
        }

        [HttpGet]
        [Route( "Recipe/FeedSearch/{pageNumber}" )]
        public async Task<List<RecipeFeedDto>> GetFeedBySearchString( int pageNumber, string search )
        {
            //TODO: add userId
            int userId = 1;

            return await _recipeService.GetRecipesFeedBySearchStringAsync( pageNumber, userId, search );
        }

        [HttpGet]
        [Route( "Recipe/User/{pageNumber}" )]
        public async Task<List<RecipeFeedDto>> GetUserFeed( int pageNumber )
        {
            //TODO: add userId
            int userId = 1;

            return await _recipeService.GetRecipesFeedByUserIdAsync( pageNumber, userId );
        }

        [HttpGet]
        public async Task<List<RecipeFeedDto>> GetFavorites()
        {
            //TODO: add userId
            int userId = 1;

            return await _recipeService.GetFavoriteFeedByUserIdAsync( userId );
        }
    }
}
