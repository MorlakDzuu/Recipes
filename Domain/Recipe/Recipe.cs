﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Recipe
{
    public class Recipe
    {
        public int Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CookingTime { get; set; }
        public int PortionsCount { get; set; }
        public string PhotoUrl { get; set; }
        public List<Stage> Stages { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public int UserId { get; set; }

        public Recipe( string title, string description, int cookingTime, int portionsCount, string photoUrl, List<Stage> stages, List<Ingredient> ingredients, int userId )
        {
            Title = title;
            Description = description;
            CookingTime = cookingTime;
            PortionsCount = portionsCount;
            PhotoUrl = photoUrl;
            Stages = stages;
            Ingredients = ingredients;
            UserId = userId;
        }
    }
}
