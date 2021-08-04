using Application.Dto;
using Domain.Favorite;
using Domain.Like;
using Domain.Recipe;
using Domain.Tag;
using Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public interface IRecipeService
    {
        public List<Recipe> GetByUserIdAsync( int userId );
        public Task AddRecipeAsync( RecipeDto recipeDto, int userId );
        public Task<List<Recipe>> GetAllAsync();
        public Task<RecipePageDto> GetRecipeById( int recipeId, int userId );
        public Task AddLikeRecipeByUserAsync( int userId, int recipeId );
        public Task AddFavoriteRecipeByUser( int userId, int recipeId );
    }

    public class RecipeService : IRecipeService     
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly ITagService _tagService;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RecipeService( IRecipeRepository recipeRepository, ITagService tagService, IFavoriteRepository favoriteRepository, ILikeRepository likeRepository, IUnitOfWork unitOfWork )
        {
            _recipeRepository = recipeRepository;
            _tagService = tagService;
            _favoriteRepository = favoriteRepository;
            _likeRepository = likeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddRecipeAsync( RecipeDto recipeDto, int userId )
        {
            Recipe recipe = new Recipe( recipeDto.Title, recipeDto.Description, recipeDto.CookingTime, recipeDto.PortionsCount, recipeDto.PhotoUrl, recipeDto.Stages, recipeDto.Ingredients, userId );
            await _recipeRepository.AddAsync( recipe );
            await _unitOfWork.Commit();

            foreach( string tag in recipeDto.Tags )
            {
                await _tagService.AddToRecipeAsync( tag, recipe.Id );
            }
        }

        public async Task<List<Recipe>> GetAllAsync()
        {
            return await _recipeRepository.GetAllAsync();
        }

        public List<Recipe> GetByUserIdAsync( int userId )
        {
            throw new NotImplementedException();
        }

        public async Task<RecipePageDto> GetRecipeById( int recipeId, int userId )
        {
            Recipe recipe = await _recipeRepository.GetAsync( recipeId );
            List<string> tags = await _tagService.GetTagsByRecipeId( recipeId );
            int favoritesCount = await _favoriteRepository.GetFavoriteCountByRecipeId( recipeId );
            int likesCount = await _likeRepository.GetCountByRecipeIdAsync( recipeId );
            bool isLiked = await _likeRepository.IsRecipeLikedByUser( recipeId, userId );

            RecipePageDto recipePageDto = new RecipePageDto { Title = recipe.Title, Description = recipe.Description, CookingTime = recipe.CookingTime, PortionsCount = recipe.PortionsCount,
                                                              FavoritesCount = favoritesCount, LikesCount = likesCount, PhotoUrl = recipe.PhotoUrl, Stages = recipe.Stages, 
                                                              Ingredients = recipe.Ingredients, Tags = tags, IsLiked = isLiked };

            return recipePageDto;
        }

        public async Task AddLikeRecipeByUserAsync( int userId, int recipeId )
        {
            await _likeRepository.AddAsync( userId, recipeId );
            await _unitOfWork.Commit();
        }

        public async Task AddFavoriteRecipeByUser( int userId, int recipeId )
        {
            await _favoriteRepository.AddAsync( userId, recipeId );
            await _unitOfWork.Commit();
        }
    }
}
