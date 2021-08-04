﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Like
{
    public interface ILikeRepository
    {
        public Task AddAsync( int userId, int recipeId );
        public void Delete( int userId, int recipeId );
        public Task<int> GetCountByRecipeIdAsync( int recipeId );
        public Task<bool> IsRecipeLikedByUser( int recipeId, int userId );
    }
}