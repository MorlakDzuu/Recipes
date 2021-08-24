using Application.Dto;
using Application.Service;
using Domain.Label;
using Domain.Recipe;
using Infastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Extranet.Api.Controllers
{
    [Route( "[controller]" )]
    public class RecipeController : Controller
    {
        private readonly IRecipeService _recipeService;
        private readonly ITagService _tagService;
        private readonly ILabelRepository _labelRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RecipeController(
            IRecipeService recipeService,
            ITagService tagService,
            ILabelRepository labelRepository,
            IRecipeRepository recipeRepository,
            IUnitOfWork unitOfWork )
        {
            _recipeService = recipeService;
            _tagService = tagService;
            _labelRepository = labelRepository;
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
        }

        [Authorize( AuthenticationSchemes = "Bearer" )]
        [HttpPost, Route( "add" )]
        public async Task<IActionResult> AddRecipe( [FromBody] RecipeDto recipeDto )
        {
            int userId = int.Parse( User.Claims.First( c => c.Type == ClaimTypes.NameIdentifier ).Value );

            try
            {
                Recipe recipe = await _recipeService.AddRecipeAsync( recipeDto, userId );
                await _tagService.AddNewTagsAsync( recipeDto.Tags );
                await _unitOfWork.Commit();

                await _tagService.AddTagsToRecipeAsync( recipeDto.Tags, recipe.Id );
                await _unitOfWork.Commit();

                return Ok();
            } catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpGet, Route( "get/{recipeId}" )]
        public async Task<RecipePageDto> Get( int recipeId )
        {
            int userId = 0;
            if ( User.Identity.IsAuthenticated )
            {
                userId = int.Parse( User.Claims.First( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            }

            return await _recipeService.GetRecipeById( recipeId, userId );
        }

        [Authorize( AuthenticationSchemes = "Bearer" )]
        [HttpGet, Route( "like/add/{recipeId}" )]
        public async Task<IActionResult> AddLike( int recipeId )
        {
            int userId = int.Parse( User.Claims.First( c => c.Type == ClaimTypes.NameIdentifier ).Value );

            try
            {
                await _labelRepository.AddLikeAsync( userId, recipeId );
                await _unitOfWork.Commit();

                return Ok( "success" );
            }
            catch ( Exception )
            {
                return BadRequest( "error" );
            }
        }

        [Authorize( AuthenticationSchemes = "Bearer" )]
        [HttpGet, Route( "favorite/add/{recipeId}" )]
        public async Task<IActionResult> AddFavorite( int recipeId )
        {
            int userId = int.Parse( User.Claims.First( c => c.Type == ClaimTypes.NameIdentifier ).Value );

            try
            {
                await _labelRepository.AddFavoriteAsync( userId, recipeId );
                await _unitOfWork.Commit();

                return Ok( "success" );
            }
            catch ( Exception )
            {
                return BadRequest( "error" );
            }
        }

        [Authorize( AuthenticationSchemes = "Bearer" )]
        [HttpGet, Route( "like/delete/{recipeId}" )]
        public async Task<IActionResult> DeleteLike( int recipeId )
        {
            int userId = int.Parse( User.Claims.First( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            try
            {
                await _recipeService.DeleteLikeByUserAsync( userId, recipeId );
                await _unitOfWork.Commit();
                
                return Ok( "success" );
            }
            catch ( Exception )
            {
                return BadRequest( "error" );
            }
        }

        [Authorize( AuthenticationSchemes = "Bearer" )]
        [HttpGet, Route( "favorite/delete/{recipeId}" )]
        public async Task<IActionResult> DeleteFavorite( int recipeId )
        {
            int userId = int.Parse( User.Claims.First( c => c.Type == ClaimTypes.NameIdentifier ).Value );

            try
            {
                await _recipeService.DeleteFavoriteByUserAsync( userId, recipeId );
                await _unitOfWork.Commit();

                return Ok( "success" );
            }
            catch ( Exception )
            {
                return BadRequest( "error" );
            }
        }

        [HttpPost, Route( "edit/{recipeId}" )]
        public async Task EditRecipe( [FromBody] RecipeDto recipeDto, int recipeId )
        {
            Recipe recipe = await _recipeRepository.GetAsync( recipeId );

            recipe.Title = recipeDto.Title;
            recipe.Description = recipeDto.Description;
            recipe.CookingTime = recipeDto.CookingTime;
            recipe.PortionsCount = recipeDto.PortionsCount;
            recipe.PhotoUrl = recipeDto.PhotoUrl;
            recipe.Stages = recipeDto.Stages.ConvertAll( item => new Stage() { SerialNumber = item.SerialNumber, Description = item.Description } );
            recipe.Ingredients = recipeDto.Ingredients.ConvertAll( item => new Ingredient() { Title = item.Title, Description = item.Description } );

            await _tagService.AddNewTagsAsync( recipeDto.Tags );
            await _unitOfWork.Commit();

            await _tagService.DeleteOldTagsAsync( recipeId, recipeDto.Tags );
            await _tagService.AddTagsToRecipeAsync( recipeDto.Tags, recipeId );
            await _unitOfWork.Commit();
        }

        [HttpGet, Route( "delete/{recipeId}" )]
        public async Task DeleteRecipe( int recipeId )
        {
            await _tagService.DeleteOldTagsAsync( recipeId );
            await _recipeRepository.DeleteAsync( recipeId );
            await _unitOfWork.Commit();
        }

        [HttpGet, Route( "feed/{pageNumber}" )]
        public async Task<List<RecipeFeedDto>> GetFeed( int pageNumber )
        {
            int userId = 0;
            if ( User.Identity.IsAuthenticated )
            {
                userId = int.Parse( User.Claims.First( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            }

            return await _recipeService.GetRecipesFeedAsync( pageNumber, userId );
        }

        [HttpGet, Route( "feed/search/{pageNumber}" )]
        public async Task<List<RecipeFeedDto>> GetFeedBySearchString( int pageNumber, string search )
        {
            int userId = 0;
            if ( User.Identity.IsAuthenticated )
            {
                userId = int.Parse( User.Claims.First( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            }

            return await _recipeService.GetRecipesFeedAsync( pageNumber, userId, searchString: search );
        }

        [Authorize( AuthenticationSchemes = "Bearer" )]
        [HttpGet, Route( "feed/user/{pageNumber}" )]
        public async Task<List<RecipeFeedDto>> GetUserFeed( int pageNumber )
        {
            int userId = int.Parse( User.Claims.First( c => c.Type == ClaimTypes.NameIdentifier ).Value );

            return await _recipeService.GetRecipesFeedAsync( pageNumber, userId, orderByUser: true );
        }

        [Authorize( AuthenticationSchemes = "Bearer" )]
        [HttpGet, Route( "feed/favorite/{pageNumber}" )]
        public async Task<List<RecipeFeedDto>> GetFavorites( int pageNumber )
        {
            int userId = int.Parse( User.Claims.First( c => c.Type == ClaimTypes.NameIdentifier ).Value );

            return await _recipeService.GetRecipesFeedAsync( pageNumber, userId, orderByFavorite: true );
        }

        [HttpGet, Route( "feed/recipeOfDay" )]
        public async Task<RecipeFeedDto> GetRecipeOfDay()
        {
            int userId = 0;
            if ( User.Identity.IsAuthenticated )
            {
                userId = int.Parse( User.Claims.First( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            }

            return await _recipeService.GetRecipeOfDay( userId );
        }
    }
}
