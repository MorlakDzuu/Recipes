using Domain.Favorite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Configuration
{
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.Property(item => item.UserId)
               .IsRequired()
               .HasColumnName("user_id");

            builder.Property(item => item.RecipeId)
                .IsRequired()
                .HasColumnName("recipe_id");

            builder.HasKey(item => new { item.UserId, item.RecipeId });
        }
    }
}
