using Application.Dto;
using Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public interface IUserService
    {
        public List<UserDto> GetAllUsers();
        public void AddNewUser(string name, string login, string description, string password);
    }
    public class UserService : IUserService
    {
        private IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public List<UserDto> GetAllUsers()
        {
            List<UserDto> users = userRepository.GetAll().Select(item => new UserDto() { Name = item.Name, Login = item.Login, Description = item.Description }).ToList();
            return users;
        }

        public void AddNewUser(string name, string login, string description, string password)
        {

        }
    }
}
