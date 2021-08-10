using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.label
{
    public interface ILabelRepository
    {
        public Task AddLikeAsync( int userId, int recipeId );
        public Task DeleteLikeAsync( int userId, int recipeId );
        public Task AddFavoriteAsync( int userId, int recipeId );
        public Task DeleteFavoriteAsync( int userId, int recipeId );
        public Task<List<Recipe.Recipe>> GetFavoriteRecipesByUserIdAsync( int userId );
        public Task<int> GetFavoriteCountByRecipeIdAsync( int recipeId );
        public Task<int> GetLikeCountByRecipeIdAsync( int recipeId );
        public Task<bool> IsRecipeLikedByUser( int recipeId, int userId );
    }
}
