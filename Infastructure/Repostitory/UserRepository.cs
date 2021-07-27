using Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repostitory
{
    public class UserRepository : IUserRepository
    {
        private ApplicationContext _applicationContext;

        public UserRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        public User Get()
        {
            throw new NotImplementedException();
        }

        public void AddUser(User user)
        {
            _applicationContext.Users.Add(user);
        }

        public List<User> GetAll()
        {
            return _applicationContext.Users.ToList();
        }
    }
}
