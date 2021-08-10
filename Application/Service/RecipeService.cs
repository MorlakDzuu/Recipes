using Application.Dto;
using Domain.label;
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
        public Task<Recipe> AddRecipeAsync( RecipeDto recipeDto, int userId );
        public Task<List<RecipePageDto>> GetAllAsync();
        public Task<RecipePageDto> GetRecipeById( int recipeId, int userId );
        public Task AddLikeRecipeByUserAsync( int userId, int recipeId );
        public Task AddFavoriteRecipeByUserAsync( int userId, int recipeId );
        public Task DeleteLikeByUserAsync( int userId, int recipeId );
        public Task DeleteFavoriteByUserAsync( int userId, int recipeId );
        public Task EditRecipeAsync( RecipeDto recipeDto, int recipeId );
        public Task DeleteRecipeByIdAsync( int recipeId );
        public Task<List<RecipeFeedDto>> GetRecipesFeedAsync( int pageNumber, int userId );
        public Task<List<RecipeFeedDto>> GetRecipesFeedBySearchStringAsync( int pageNumber, int userId, string searchString );
        public Task<List<RecipeFeedDto>> GetRecipesFeedByUserIdAsync( int pageNumber, int userId );
        public Task<List<RecipeFeedDto>> GetFavoriteFeedByUserIdAsync( int userId );
    }

    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly ITagService _tagService;
        private readonly ILabelRepository _labelRepository;

        public RecipeService
            (
            IRecipeRepository recipeRepository,
            ITagService tagService,
            ILabelRepository labelRepository
            )
        {
            _recipeRepository = recipeRepository;
            _tagService = tagService;
            _labelRepository = labelRepository;
        }

        public async Task<Recipe> AddRecipeAsync( RecipeDto recipeDto, int userId )
        {
            Recipe recipe = new Recipe
                (
                recipeDto.Title,
                recipeDto.Description,
                recipeDto.CookingTime,
                recipeDto.PortionsCount,
                recipeDto.PhotoUrl,
                recipeDto.Stages,
                recipeDto.Ingredients,
                userId
                );
            await _recipeRepository.AddAsync( recipe );
            return recipe;
        }

        public async Task<List<RecipePageDto>> GetAllAsync()
        {
            List<int> recipesIds = await _recipeRepository.GetAllIdsAsync();
            List<RecipePageDto> recipePageDtos = new List<RecipePageDto>();
            int userId = 1;

            foreach ( int recipeId in recipesIds )
            {
                RecipePageDto recipePageDto = await GetRecipeById( recipeId, userId );
                recipePageDtos.Add( recipePageDto );
            }

            return recipePageDtos;
        }

        public List<Recipe> GetByUserIdAsync( int userId )
        {
            throw new NotImplementedException();
        }

        public async Task<RecipePageDto> GetRecipeById( int recipeId, int userId )
        {
            Recipe recipe = await _recipeRepository.GetAsync( recipeId );
            List<string> tags = await _tagService.GetTagsByRecipeId( recipeId );
            int favoritesCount = await _labelRepository.GetFavoriteCountByRecipeIdAsync( recipeId );
            int likesCount = await _labelRepository.GetLikeCountByRecipeIdAsync( recipeId );
            bool isLiked = await _labelRepository.IsRecipeLikedByUser( recipeId, userId );

            RecipePageDto recipePageDto = new RecipePageDto
            {
                Title = recipe.Title,
                Description = recipe.Description,
                CookingTime = recipe.CookingTime,
                PortionsCount = recipe.PortionsCount,
                FavoritesCount = favoritesCount,
                LikesCount = likesCount,
                PhotoUrl = recipe.PhotoUrl,
                Stages = recipe.Stages,
                Ingredients = recipe.Ingredients,
                Tags = tags,
                IsLiked = isLiked
            };

            return recipePageDto;
        }

        public async Task AddLikeRecipeByUserAsync( int userId, int recipeId )
        {
            await _labelRepository.AddLikeAsync( userId, recipeId );
        }

        public async Task AddFavoriteRecipeByUserAsync( int userId, int recipeId )
        {
            await _labelRepository.AddFavoriteAsync( userId, recipeId );
        }

        public async Task EditRecipeAsync( RecipeDto recipeDto, int recipeId )
        {
            Recipe recipe = await _recipeRepository.GetAsync( recipeId );

            recipe.Title = recipeDto.Title;
            recipe.Description = recipeDto.Description;
            recipe.CookingTime = recipeDto.CookingTime;
            recipe.PortionsCount = recipeDto.PortionsCount;
            recipe.PhotoUrl = recipeDto.PhotoUrl;
            recipe.Stages = recipeDto.Stages;
            recipe.Ingredients = recipeDto.Ingredients;
        }

        public async Task DeleteRecipeByIdAsync( int recipeId )
        {
            await _tagService.DeleteTagsFromRecipeAsyc( recipeId );
            await _recipeRepository.DeleteAsync( recipeId );
        }

        private async Task<List<RecipeFeedDto>> ConvertRecipesToRecipeFeedDtos( List<Recipe> recipes, int userId )
        {
            List<RecipeFeedDto> recipeFeedDtos = new List<RecipeFeedDto>();
            foreach ( Recipe recipe in recipes )
            {
                List<string> tags = await _tagService.GetTagsByRecipeId( recipe.Id );
                int favoritesCount = await _labelRepository.GetFavoriteCountByRecipeIdAsync( recipe.Id );
                int likesCount = await _labelRepository.GetLikeCountByRecipeIdAsync( recipe.Id );
                bool isLiked = await _labelRepository.IsRecipeLikedByUser( recipe.Id, userId );

                RecipeFeedDto recipeFeedDto = new RecipeFeedDto()
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    Description = recipe.Description,
                    CookingTime = recipe.CookingTime,
                    PortionsCount = recipe.PortionsCount,
                    PhotoUrl = recipe.PhotoUrl,
                    Tags = tags,
                    FavoritesCount = favoritesCount,
                    LikesCount = likesCount,
                    IsLiked = isLiked
                };

                recipeFeedDtos.Add( recipeFeedDto );
            }
            return recipeFeedDtos;
        }

        public async Task<List<RecipeFeedDto>> GetRecipesFeedAsync( int pageNumber, int userId )
        {
            List<Recipe> recipes = await _recipeRepository.GetUsingPaginationAsync( pageNumber );
            return await ConvertRecipesToRecipeFeedDtos( recipes, userId );
        }

        public async Task<List<RecipeFeedDto>> GetRecipesFeedBySearchStringAsync( int pageNumber, int userId, string searchString )
        {
            List<Recipe> recipes = await _recipeRepository.GetUsingPaginationBySearchStringAsync( pageNumber, searchString );
            return await ConvertRecipesToRecipeFeedDtos( recipes, userId );
        }

        public async Task<List<RecipeFeedDto>> GetRecipesFeedByUserIdAsync( int pageNumber, int userId )
        {
            List<Recipe> recipes = await _recipeRepository.GetUsingPaginationByUserIdAsync( pageNumber, userId );
            List<RecipeFeedDto> recipeFeedDtos = new List<RecipeFeedDto>();
            return await ConvertRecipesToRecipeFeedDtos( recipes, userId );
        }

        public async Task<List<RecipeFeedDto>> GetFavoriteFeedByUserIdAsync( int userId )
        {
            List<Recipe> recipes = await _labelRepository.GetFavoriteRecipesByUserIdAsync( userId );
            return await ConvertRecipesToRecipeFeedDtos( recipes, userId );
        }

        public async Task DeleteLikeByUserAsync( int userId, int recipeId )
        {
            await _labelRepository.DeleteLikeAsync( userId, recipeId );
        }

        public async Task DeleteFavoriteByUserAsync( int userId, int recipeId )
        {
            await _labelRepository.DeleteFavoriteAsync( userId, recipeId );
        }
    }
}
