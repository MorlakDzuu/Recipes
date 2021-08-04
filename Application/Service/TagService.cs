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
        public Task AddToRecipeAsync( string name, int recipeId );
        public Task<List<string>> GetTagsByRecipeId( int recipeId );
    }
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TagService( ITagRepository tagRepository, IUnitOfWork unitOfWork )
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddToRecipeAsync( string name, int recipeId )
        {
            Tag tag = await _tagRepository.GetTagByName( name );

            if ( tag == null )
            {
                tag = new Tag( name );
                await _tagRepository.AddTagAsync( tag );
                await _unitOfWork.Commit();
            }

            TagToRecipe tagToRecipe = new TagToRecipe( tag.Id, recipeId );
            await _tagRepository.AddTagToRecipeAsync( tagToRecipe );
            await _unitOfWork.Commit();
        }

        public async Task<List<string>> GetTagsByRecipeId( int recipeId )
        {
            List<Tag> tags = await _tagRepository.GetTagsByRecipeIdAsync( recipeId );
            return tags.ConvertAll( item => item.Name );
        }
    }
}
