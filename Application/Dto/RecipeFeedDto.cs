﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class RecipeFeedDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CookingTime { get; set; }
        public int PortionsCount { get; set; }
        public string PhotoUrl { get; set; }
        public int FavoritesCount { get; set; }
        public int LikesCount { get; set; }
        public List<string> Tags { get; set; }
        public bool IsLiked { get; set; }
    }
}