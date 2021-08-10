using Domain.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repostitory
{
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<User> _usersDbSet;

        public UserRepository( ApplicationContext applicationContext )
        {
            _usersDbSet = applicationContext.Set<User>();
        }

        public async Task<User> GetAsync( int id )
        {
            return await _usersDbSet.FindAsync( id );
        }

        public async Task AddUserAsync( User user )
        {
            await _usersDbSet.AddAsync( user );
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _usersDbSet.ToListAsync();
        }

        public async Task<User> GetByLoginAsync( string login )
        {
            return await _usersDbSet.Where( item => item.Login == login ).SingleOrDefaultAsync();
        }
    }
}
