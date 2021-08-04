using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tag
{
    public class TagToRecipe
    {
        public int TagId { get; set; }
        public int RecipeId { get; set; }

        public TagToRecipe( int tagId, int recipeId )
        {
            TagId = tagId;
            RecipeId = recipeId;
        }
    }
}
