using Domain.Recipe;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repostitory
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly DbSet<Recipe> _recipesDbSet;

        public RecipeRepository( ApplicationContext applicationContext )
        {
            _recipesDbSet = applicationContext.Set<Recipe>();
        }

        public async Task<List<Recipe>> GetAllAsync()
        {
            return await _recipesDbSet.ToListAsync();
        }

        public async Task<Recipe> GetAsync( int id )
        {
            return await _recipesDbSet.FindAsync( id );
        }
    }
}
