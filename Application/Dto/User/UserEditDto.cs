using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.User
{
    public class UserEditDto
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
    }
}
