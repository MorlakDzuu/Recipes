using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.User
{
    public interface IUserRepository
    {
        public User Get();
        public List<User> GetAll();

        public void AddUser(User user);
    }
}
