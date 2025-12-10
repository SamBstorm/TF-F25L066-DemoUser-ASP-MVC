using DemoUser.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace DemoUser.Domain.Repositories
{
    public interface IUserRepository<TUser> : ICRUDRepository<TUser, Guid> where TUser : class
    {
        User? CheckPassword(string email, string password);
    }
}
