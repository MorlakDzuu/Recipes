﻿using Domain.Tag;
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
        public Task DeleteOldTagsAsync( List<string> newTags, int recipeId );
        public Task DeleteTagsFromRecipeAsyc( int recipeId );
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
                Tag tag = await _tagRepository.GetTagByNameAsync( tagStr );

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
                Tag tag = await _tagRepository.GetTagByNameAsync( tagStr );
                bool isConnectExist = await _tagRepository.IsTagToRecipeExistAsync( tag.Id, recipeId );

                if ( !isConnectExist )
                {
                    TagToRecipe tagToRecipe = new TagToRecipe( tag.Id, recipeId );
                    await _tagRepository.AddTagToRecipeAsync( tagToRecipe );
                }
            }
        }

        public async Task DeleteTagsFromRecipeAsyc( int recipeId )
        {
            List<string> tags = new List<string>();
            await DeleteOldTagsAsync( tags, recipeId );
        }

        public async Task DeleteOldTagsAsync( List<string> newTags, int recipeId )
        {
            List<Tag> oldTags = await _tagRepository.GetTagsByRecipeIdAsync( recipeId );

            List<string> oldTagsStr = oldTags.ConvertAll( item => item.Name );
            foreach ( string tag in oldTagsStr )
            {
                if ( !newTags.Contains( tag ) )
                    await _tagRepository.DeleteTagFromRecipeAsync( tag, recipeId );
            }
        }

        public async Task<List<string>> GetTagsByRecipeId( int recipeId )
        {
            List<Tag> tags = await _tagRepository.GetTagsByRecipeIdAsync( recipeId );
            return tags.ConvertAll( item => item.Name );
        }
    }
}
