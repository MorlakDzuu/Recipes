using Domain.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public interface IRecipeService
    {
        public List<Recipe> GetRecipesByUserId(int userId); 
    }
    public class RecipeService : IRecipeService
    {
        private IRecipeRepository _recipeRepository;

        public RecipeService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }
        public List<Recipe> GetRecipesByUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
