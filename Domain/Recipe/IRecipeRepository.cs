using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Recipe
{
    public interface IRecipeRepository
    {
        public Recipe Get();
        public List<Recipe> GetAll();   
    }
}
