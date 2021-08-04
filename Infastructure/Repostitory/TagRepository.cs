using Domain.Tag;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repostitory
{
    public class TagRepository : ITagRepository
    {
        private readonly DbSet<Tag> _tagsDbSet;
        private readonly DbSet<TagToRecipe> _tagToRecipeDbSet;

        public TagRepository(ApplicationContext applicationContext)
        {
            _tagsDbSet = applicationContext.Set<Tag>();
            _tagToRecipeDbSet = applicationContext.Set<TagToRecipe>();
        }

        public async Task AddTagAsync( Tag tag )
        {
            await _tagsDbSet.AddAsync( tag );
        }

        public async Task AddTagToRecipeAsync( TagToRecipe tagToRecipe )
        {
            await _tagToRecipeDbSet.AddAsync( tagToRecipe );
        }

        public Task DeleteTagAsync( int tagId )
        {
            throw new NotImplementedException();
        }

        public Task DeleteTagFromRecipeAsync( int tagId, int recipeId )
        {
            throw new NotImplementedException();
        }

        public Task<List<Tag>> GetAllTagsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Tag> GetTagByName(string name)
        {
            return await _tagsDbSet.Where( item => item.Name == name ).SingleOrDefaultAsync();
        }

        public async Task<List<Tag>> GetTagsByRecipeIdAsync( int recipeId )
        {
            return await _tagToRecipeDbSet
                .Where( item => item.RecipeId == recipeId )
                .Join( _tagsDbSet,
                tagToRecipe => tagToRecipe.TagId,
                tag => tag.Id,
                (tagToRecipe, tag) => tag)
                .ToListAsync();
        }

        public Task<List<Tag>> GetTagsByStringAsync( string start )
        {
            throw new NotImplementedException();
        }
    }
}
