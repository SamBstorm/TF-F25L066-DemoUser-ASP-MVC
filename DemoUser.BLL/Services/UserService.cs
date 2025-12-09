using DemoUser.BLL.Entities;
using DemoUser.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoUser.BLL.Services
{
    public class UserService : IUserRepository<User>
    {
        private IUserRepository<DAL.Entities.User> _repository;

        public UserService(IUserRepository<DAL.Entities.User> repository)
        {
            _repository = repository;
        }

        public Guid CheckPassword(string email, string password)
        {
            return _repository.CheckPassword(email, password);
        }

        public void Delete(Guid id)
        {
            _repository.Delete(id);
        }

        public IEnumerable<User> Get()
        {
            throw new NotImplementedException();
        }

        public User Get(Guid id)
        {
            return _repository.Get(id).ToBLL();
        }

        public Guid Insert(User entity)
        {
            return _repository.Insert(entity.ToDAL());
        }

        public void Update(Guid id, User entity)
        {
            throw new NotImplementedException();
        }
    }
}
