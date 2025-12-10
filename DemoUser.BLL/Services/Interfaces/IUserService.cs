using System;
using System.Collections;
using System.Collections.Generic;
using DemoUser.Domain.Entities;

namespace DemoUser.BLL.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> Get();
        User? Get(Guid id);
        Guid Register(string email, string password);
        User? Login(string email, string password);
        void Disable(Guid id);
    }
}