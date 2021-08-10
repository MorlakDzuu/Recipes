using Domain.label;
using Domain.Recipe;
using Domain.Tag;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repostitory
{
    public class RecipeRepository : IRecipeRepository
    {
        private const int PAGE_SIZE = 4;
        private readonly DbSet<Recipe> _recipesDbSet;
        private readonly DbSet<TagToRecipe> _tagToRecipesDbSet;
        private readonly DbSet<Tag> _tagsDbSet;
        private readonly DbSet<Label> _labelDbSet;

        public RecipeRepository( ApplicationContext applicationContext )
        {
            _recipesDbSet = applicationContext.Set<Recipe>();
            _tagToRecipesDbSet = applicationContext.Set<TagToRecipe>();
            _tagsDbSet = applicationContext.Set<Tag>();
            _labelDbSet = applicationContext.Set<Label>();
        }

        public async Task AddAsync( Recipe recipe )
        {
            await _recipesDbSet.AddAsync( recipe );
        }

        public async Task DeleteAsync( int id )
        {
            Recipe recipe = await GetAsync( id );
            _recipesDbSet.Remove( recipe );
        }

        public async Task<List<Recipe>> GetAllAsync()
        {
            return await _recipesDbSet.ToListAsync();
        }

        public async Task<List<int>> GetAllIdsAsync()
        {
            return await _recipesDbSet.Select( recipe => recipe.Id ).ToListAsync();
        }

        public async Task<Recipe> GetAsync( int id )
        {
            return await _recipesDbSet.Where( item => item.Id == id ).SingleOrDefaultAsync();
        }

        public async Task<List<Recipe>> GetUsingPaginationAsync( int pageNumber )
        {
            return await _recipesDbSet.Skip( PAGE_SIZE * ( pageNumber - 1 ) ).Take( PAGE_SIZE ).ToListAsync();
        }

        public async Task<List<Recipe>> GetUsingPaginationBySearchStringAsync( int pageNumber, string searchString )
        {
            List<int> recipesIds = await _recipesDbSet.Join(
                _tagToRecipesDbSet.DefaultIfEmpty(),
                recipe => recipe.Id,
                tagToRecipe => tagToRecipe.RecipeId,
                ( recipe, tagToRecipe ) => new
                {
                    RecipeId = recipe.Id,
                    Title = recipe.Title,
                    TagId = tagToRecipe.TagId
                } )
                .Join(
                _tagsDbSet.DefaultIfEmpty(),
                combinedEntity => combinedEntity.TagId,
                tag => tag.Id,
                ( combinedEntity, tag ) => new
                {
                    RecipeId = combinedEntity.RecipeId,
                    Title = combinedEntity.Title,
                    TagName = tag.Name
                } )
                .Where( item => ( item.Title.Contains( searchString ) ) || ( item.TagName.Contains( searchString ) ) )
                .Select( item => item.RecipeId )
                .Distinct()
                .Skip( PAGE_SIZE * ( pageNumber - 1 ) )
                .Take( PAGE_SIZE )
                .ToListAsync();

            return await _recipesDbSet.Where( item => recipesIds.Contains( item.Id ) ).ToListAsync();
        }

        public async Task<List<Recipe>> GetFavoriteRecipesByUserIdAsync( int userId )
        {
            return await _labelDbSet
               .Where( item => ( item.UserId == userId ) && ( item.Type == LabelTypes.Favorite ) )
               .Join( _recipesDbSet,
               favorite => favorite.RecipeId,
               recipe => recipe.Id,
               ( favorite, recipe ) => recipe )
               .ToListAsync();
        }

        public async Task<List<Recipe>> GetUsingPaginationByUserIdAsync( int pageNumber, int userId )
        {
            return await _recipesDbSet.Where( item => item.UserId == userId ).Skip( PAGE_SIZE * ( pageNumber - 1 ) ).Take( PAGE_SIZE ).ToListAsync();
        }
    }
}
