using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Favorite
{
    public class Favorite
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }

        public Favorite( int userId, int recipeId )
        {
            UserId = userId;
            RecipeId = recipeId;
        }
    }
}
