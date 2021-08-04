using Domain.Favorite;
using Domain.Recipe;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repostitory
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly DbSet<Favorite> _favoriteDbSet;
        private readonly DbSet<Recipe> _recipeDbSet;

        public FavoriteRepository(ApplicationContext applicationContext)
        {
            _favoriteDbSet = applicationContext.Set<Favorite>();
            _recipeDbSet = applicationContext.Set<Recipe>();
        }

        public async Task AddAsync( int userId, int recipeId )
        {
            Favorite favorite = new Favorite( userId, recipeId );
            await _favoriteDbSet.AddAsync( favorite );
        }

        public void Delete( int userId, int recipeId )
        {
            Favorite favorite = new Favorite( userId, recipeId );
            _favoriteDbSet.Remove( favorite );
        }

        public async Task<int> GetFavoriteCountByRecipeId(int recipeId)
        {
            return await _favoriteDbSet.Where( item => item.RecipeId == recipeId ).CountAsync();
        }

        public async Task<List<Recipe>> GetRecipesByUserIdAsync( int userId )
        {
            return await _favoriteDbSet
                .Where( item => item.UserId == userId )
                .Join( _recipeDbSet, 
                favorite => favorite.RecipeId,
                recipe => recipe.Id,
                ( favorite, recipe ) => recipe )
                .ToListAsync();
        }
    }
}
