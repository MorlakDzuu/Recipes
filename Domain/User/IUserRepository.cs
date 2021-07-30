using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.User
{
    public interface IUserRepository
    {
        public Task<User> GetAsync( int id );
        public Task<List<User>> GetAllAsync();
        public Task AddUserAsync( User user );
    }
}
