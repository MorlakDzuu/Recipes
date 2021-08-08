using Domain.Tag;
using Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public interface ITagService
    {
        public Task AddTagsToRecipeAsync( List<string> tags, int recipeId );
        public Task<List<string>> GetTagsByRecipeId( int recipeId );
        public Task AddNewTagsAsync( List<string> tags );
    }
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService( ITagRepository tagRepository )
        {
            _tagRepository = tagRepository;
        }

        public async Task AddNewTagsAsync( List<string> tags )
        {
            foreach ( string tagStr in tags )
            {
                Tag tag = await _tagRepository.GetTagByName( tagStr );

                if ( tag == null )
                {
                    tag = new Tag( tagStr );
                    await _tagRepository.AddTagAsync( tag );
                }
            }
        }

        public async Task AddTagsToRecipeAsync( List<string> tags, int recipeId )
        {
            foreach ( string tagStr in tags )
            {
                Tag tag = await _tagRepository.GetTagByName( tagStr );

                TagToRecipe tagToRecipe = new TagToRecipe( tag.Id, recipeId );
                await _tagRepository.AddTagToRecipeAsync( tagToRecipe );
            }
        }

        public async Task<List<string>> GetTagsByRecipeId( int recipeId )
        {
            List<Tag> tags = await _tagRepository.GetTagsByRecipeIdAsync( recipeId );
            return tags.ConvertAll( item => item.Name );
        }
    }
}
