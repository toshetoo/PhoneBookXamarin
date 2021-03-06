﻿using PhoneBook.Entities;
using PhoneBook.Repositories;
using PhoneBook.Services.EntityServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Services
{
    public static class AuthenticationService
    {
        public static User LoggedUser { get; set; }

        public static void AuthenticateUser(string username, string password)
        {
            LoggedUser = new UsersService().GetByUsernameAndPassword(username, password);
        }
    }
}
