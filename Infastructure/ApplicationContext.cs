using Domain.User;
using Infastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure
{
    public class ApplicationContext : DbContext, IUnitOfWork
    {
        public ApplicationContext( DbContextOptions<ApplicationContext> options ) : base( options )
        { }

        public async Task Commit()
        {
            await this.SaveChangesAsync();
        }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            modelBuilder.ApplyConfiguration( new UserConiguration() );
            modelBuilder.ApplyConfiguration( new RecipeConfiguration() );
        }
    }
}
