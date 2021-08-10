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
        public Task<List<Recipe>> GetUsingPaginationAsync( int pageNumber );
        public Task<List<Recipe>> GetUsingPaginationBySearchStringAsync( int pageNumber, string searchString );
        public Task<List<Recipe>> GetUsingPaginationByUserIdAsync( int pageNumber, int userId );
        public Task<List<Recipe>> GetFavoriteRecipesByUserIdAsync( int userId );
        public Task AddAsync( Recipe recipe );
        public Task<List<int>> GetAllIdsAsync();
        public Task DeleteAsync( int id );
    }
}
