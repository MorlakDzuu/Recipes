using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Configuration
{
    public class UserConiguration : IEntityTypeConfiguration<User>
    {
        public void Configure( EntityTypeBuilder<User> builder )
        {
            builder.Property( item => item.Id )
                .IsRequired()
                .HasColumnName( "id" );

            builder.HasKey( item => item.Id );

            builder.Property( item => item.Name )
                .IsRequired()
                .HasMaxLength( 40 )
                .HasColumnName( "name" );

            builder.Property( item => item.Login )
                .IsRequired()
                .HasMaxLength( 40 )
                .HasColumnName( "login" );

            builder.HasIndex( item => item.Login ).IsUnique();

            builder.Property( item => item.Description )
                .HasMaxLength( 300 )
                .HasColumnName( "description" );

            builder.Property( item => item.Password )
                .IsRequired()
                .HasColumnName( "password" );
        }
    }
}
