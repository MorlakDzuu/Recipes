using Application.Dto;
using Domain.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Service
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetAllAsync();
        public Task AddAsync( UserRegistrationDto userRegistrationDto );
        public Task<UserDto> GetByLoginAsync( string login );
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService( IUserRepository userRepository )
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            List<User> users = await _userRepository.GetAllAsync();
            return users.ConvertAll( item => new UserDto() { Name = item.Name, Login = item.Login, Description = item.Description } );
        }

        public async Task AddAsync( UserRegistrationDto userRegistrationDto )
        {
            User user = new User( userRegistrationDto.Name, userRegistrationDto.Login, userRegistrationDto.Password );
            await _userRepository.AddUserAsync( user );
        }

        public async Task<UserDto> GetByLoginAsync( string login )
        {
            User user = await _userRepository.GetByLoginAsync( login );
            return new UserDto()
            {
                Name = user.Name,
                Login = user.Login,
                Description = user.Description
            };  
        }
    }
}
