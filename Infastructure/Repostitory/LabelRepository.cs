using Domain.label;
using Domain.Recipe;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repostitory
{
    public class LabelRepository : ILabelRepository
    {
        private const int LIKE_TYPE = 1;
        private const int FAVORITE_TYPE = 2;

        private readonly DbSet<Label> _labelDbSet;
        private readonly DbSet<Recipe> _recipeDbSet;

        public LabelRepository( ApplicationContext applicationContext )
        {
            _labelDbSet = applicationContext.Set<Label>();
            _recipeDbSet = applicationContext.Set<Recipe>();
        }

        public async Task AddFavoriteAsync( int userId, int recipeId )
        {
            Label label = new Label( userId, recipeId, FAVORITE_TYPE );
            await _labelDbSet.AddAsync( label );
        }

        public async Task AddLikeAsync( int userId, int recipeId )
        {
            Label label = new Label( userId, recipeId, LIKE_TYPE );
            await _labelDbSet.AddAsync( label );
        }

        public async Task<int> GetFavoriteCountByRecipeIdAsync( int recipeId )
        {
            return await _labelDbSet.Where( item => ( item.RecipeId == recipeId ) && ( item.Type == FAVORITE_TYPE ) ).CountAsync();
        }

        public async Task<List<Recipe>> GetFavoriteRecipesByUserIdAsync( int userId )
        {
            return await _labelDbSet
               .Where( item => ( item.UserId == userId ) && ( item.Type == FAVORITE_TYPE ) )
               .Join( _recipeDbSet,
               favorite => favorite.RecipeId,
               recipe => recipe.Id,
               ( favorite, recipe ) => recipe )
               .ToListAsync();
        }

        public async Task<int> GetLikeCountByRecipeIdAsync( int recipeId )
        {
            return await _labelDbSet.Where( item => ( item.RecipeId == recipeId ) && ( item.Type == LIKE_TYPE ) ).CountAsync();
        }

        public async Task<bool> IsRecipeLikedByUser( int recipeId, int userId )
        {
            Label label = await _labelDbSet.Where( item => ( item.RecipeId == recipeId ) && ( item.UserId == userId ) && ( item.Type == LIKE_TYPE ) ).SingleOrDefaultAsync();

            if ( label == null )
                return false;
            return true;
        }

        public void RemoveFavorite( int userId, int recipeId )
        {
            Label label = new Label( userId, recipeId, FAVORITE_TYPE );
            _labelDbSet.Remove( label );
        }

        public void RemoveLike( int userId, int recipeId )
        {
            Label label = new Label( userId, recipeId, LIKE_TYPE );
            _labelDbSet.Remove( label );
        }
    }
}
