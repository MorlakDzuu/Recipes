using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tag
{
    public interface ITagRepository
    {
        public Task AddTagAsync( Tag tag );
        public Task AddTagToRecipeAsync( TagToRecipe tagToRecipe );
        public Task DeleteTagAsync( int tagId );
        public Task<Tag> GetTagByName( string name );
        public Task<List<Tag>> GetAllTagsAsync();
        public Task<List<Tag>> GetTagsByStringAsync( string start );
        public Task<List<Tag>> GetTagsByRecipeIdAsync( int recipeId );
        public Task DeleteTagFromRecipeAsync( int tagId, int recipeId );
    }
}
