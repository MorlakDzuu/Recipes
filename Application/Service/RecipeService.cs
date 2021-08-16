using Application.Dto;
using Application.Dto.Recipe;
using Domain.Label;
using Domain.Recipe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Service
{
    public interface IRecipeService
    {
        public Task<Recipe> AddRecipeAsync( RecipeDto recipeDto, int userId );
        public Task<RecipePageDto> GetRecipeById( int recipeId, int userId );
        public Task DeleteLikeByUserAsync( int userId, int recipeId );
        public Task DeleteFavoriteByUserAsync( int userId, int recipeId );
        public Task<List<RecipeFeedDto>> GetRecipesFeedAsync( int pageNumber, int userId, string searchString = null, bool orderByUser = false, bool orderByFavorite = false );
        public Task<RecipeFeedDto> GetRecipeOfDay( int userId );
    }

    public class RecipeService : IRecipeService
    {
        private const int PAGE_SIZE = 4;

        private readonly IRecipeRepository _recipeRepository;
        private readonly ITagService _tagService;
        private readonly ILabelRepository _labelRepository;

        public RecipeService(
            IRecipeRepository recipeRepository,
            ITagService tagService,
            ILabelRepository labelRepository )
        {
            _recipeRepository = recipeRepository;
            _tagService = tagService;
            _labelRepository = labelRepository;
        }

        public async Task<Recipe> AddRecipeAsync( RecipeDto recipeDto, int userId )
        {
            Recipe recipe = new Recipe(
                recipeDto.Title,
                recipeDto.Description,
                recipeDto.CookingTime,
                recipeDto.PortionsCount,
                recipeDto.PhotoUrl,
                recipeDto.Stages.ConvertAll( item => new Stage() { SerialNumber = item.SerialNumber, Description = item.Description } ),
                recipeDto.Ingredients.ConvertAll( item => new Ingredient() { Title = item.Title, Description = item.Description } ),
                userId );
            await _recipeRepository.AddAsync( recipe );
            return recipe;
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
                Stages = recipe.Stages.ConvertAll( item => new StageDto() { SerialNumber = item.SerialNumber, Description = item.Description } ),
                Ingredients = recipe.Ingredients.ConvertAll( item => new IngredientDto() { Title = item.Title, Description = item.Description } ),
                Tags = tags,
                IsLiked = isLiked
            };

            return recipePageDto;
        }

        public async Task<List<RecipeFeedDto>> GetRecipesFeedAsync(
            int pageNumber,
            int userId,
            string searchString = null,
            bool orderByUser = false,
            bool orderByFavorite = false )
        {
            List<Recipe> recipes;

            if ( searchString != null )
            {
                searchString = searchString.ToLower().Trim();
                recipes = await _recipeRepository.GetUsingPaginationAsync( pageNumber, PAGE_SIZE, searchString: searchString );
            }
            else if ( orderByUser )
            {
                recipes = await _recipeRepository.GetUsingPaginationAsync( pageNumber, PAGE_SIZE, userId: userId );
            }
            else if ( orderByFavorite )
            {
                recipes = await _recipeRepository.GetUsingPaginationAsync( pageNumber, PAGE_SIZE, userId: userId, isFavorite: true );
            }
            else
            {
                recipes = await _recipeRepository.GetUsingPaginationAsync( pageNumber, PAGE_SIZE );
            }

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

        public async Task<RecipeFeedDto> GetRecipeOfDay( int userId )
        {
            Recipe recipe = await _recipeRepository.GetRecipeOfDay();
            List<Recipe> recipes = new List<Recipe>();
            recipes.Add( recipe );

            RecipeFeedDto recipeFeedDto = ( await ConvertRecipesToRecipeFeedDtos( recipes, userId ) )[ 0 ];

            return recipeFeedDto;
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
                    CookingDuration = recipe.CookingTime,
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
    }
}
