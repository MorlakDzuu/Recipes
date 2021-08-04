using Domain.Like;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repostitory
{
    public class LikeRepository : ILikeRepository
    {
        private readonly DbSet<Like> _likeDbSet;

        public LikeRepository( ApplicationContext applicationContext )
        {
            _likeDbSet = applicationContext.Set<Like>();
        }

        public async Task AddAsync( int userId, int recipeId )
        {
            Like like = new Like( userId, recipeId );
            await _likeDbSet.AddAsync( like );
        }

        public void Delete( int userId, int recipeId )
        {
            Like like = new Like(userId, recipeId);
            _likeDbSet.Remove( like );
        }

        public async Task<int> GetCountByRecipeIdAsync( int recipeId )
        {
            return await _likeDbSet.Where( item => item.RecipeId == recipeId ).CountAsync();
        }

        public async Task<bool> IsRecipeLikedByUser( int recipeId, int userId )
        {
            Like like = await _likeDbSet.Where( item => ( item.RecipeId == recipeId ) && ( item.UserId == userId ) ).SingleOrDefaultAsync();

            if ( like == null )
                return false;
            return true;
        }
    }
}
