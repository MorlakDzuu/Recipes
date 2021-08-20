using Application.Dto.Recipe;
using Domain.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class RecipePageDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CookingDuration { get; set; }
        public int PortionsCount { get; set; }
        public string PhotoUrl { get; set; }
        public int FavoritesCount { get; set; }
        public int LikesCount { get; set; }
        public List<string> Tags { get; set; }
        public bool IsLiked { get; set; }
        public List<IngredientDto> Ingredients { get; set; }
        public List<StageDto> Stages { get; set; }
    }
}
