using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.label
{
    public class Label
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        public int Type { get; set; }

        public Label( int userId, int recipeId, int type )
        {
            UserId = userId;
            RecipeId = recipeId;
            Type = type;
        }
    }
}
