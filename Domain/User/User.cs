﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.User
{
    public class User
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }

        public User( string name, string login, string description, string password )
        {
            Name = name;
            Login = login;
            Description = description;
            Password = password;
        }

        public User(string name, string login, string password)
        {
            Name = name;
            Login = login;
            Password = password;
        }
    }
}
