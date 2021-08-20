using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Label
{
    public interface ILabelRepository
    {
        public Task AddLikeAsync( int userId, int recipeId );
        public Task DeleteLikeAsync( int userId, int recipeId );
        public Task AddFavoriteAsync( int userId, int recipeId );
        public Task DeleteFavoriteAsync( int userId, int recipeId );
        public Task<int> GetFavoriteCountByRecipeIdAsync( int recipeId );
        public Task<int> GetLikeCountByRecipeIdAsync( int recipeId );
        public Task<bool> IsRecipeLikedByUser( int recipeId, int userId );
        public Task<bool> IsRecipeFavoriteByUser( int recipeId, int userId );
    }
}
