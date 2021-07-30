using Application.Dto;
using Domain.User;
using Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetAllAsync();
        public Task AddAsync( string name, string login, string description, string password );
    }

    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;

        public UserService( IUserRepository userRepository, IUnitOfWork unitOfWork )
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            List<User> users = await _userRepository.GetAllAsync();
            return users.ConvertAll( item => new UserDto() { Name = item.Name, Login = item.Login, Description = item.Description } );
        }

        public async Task AddAsync( string name, string login, string description, string password )
        {
            User user = new User(name, login, description, password);
            await _userRepository.AddUserAsync(user);
            await _unitOfWork.Commit();
        }
    }
}
