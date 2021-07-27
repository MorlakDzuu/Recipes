using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class UserDto
    {
        public UserDto(string name, string login, string description)
        {
            Name = name;
            Login = login;
            Description = description;
        }

        public UserDto() { }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Description { get; set; }
    }
}
