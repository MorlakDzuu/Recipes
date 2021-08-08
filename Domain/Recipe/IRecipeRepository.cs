using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Recipe
{
    public interface IRecipeRepository
    {
        public Task<Recipe> GetAsync( int id );
        public Task<List<Recipe>> GetAllAsync();
        public Task AddAsync( Recipe recipe );
        public Task<List<int>> GetAllIdsAsync();
    }
}
