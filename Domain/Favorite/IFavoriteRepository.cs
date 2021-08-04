using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Favorite
{
    public interface IFavoriteRepository
    {
        public Task AddAsync( int userId, int recipeId );
        public void Delete( int userId, int recipeId );
        public Task<List<Recipe.Recipe>> GetRecipesByUserIdAsync( int userId );
        public Task<int> GetFavoriteCountByRecipeId( int recipeId );
    }
}
