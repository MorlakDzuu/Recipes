using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Like
{
    public class Like
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }

        public Like( int userId, int recipeId )
        {
            UserId = userId;
            RecipeId = recipeId;
        }
    }
}
