using System;
using System.Collections.Generic;
using System.Net.Mail;
using DemoUser.BLL.Services.Interfaces;
using DemoUser.Domain.Entities;
using DemoUser.Domain.Repositories;

namespace DemoUser.BLL.Services.Implementations
{
    public class UserService: IUserService
    {
        private readonly IUserRepository<User> _userRepository;

        public UserService(IUserRepository<User> userRepository)
        {
            _userRepository = userRepository
                ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public IEnumerable<User> Get()
        {
            return _userRepository.Get();
        }

        public User? Get(Guid id)
        {
            return _userRepository.Get(id);
        }

        public Guid Register(string email, string password)
        {
            // exemple de validations basiques côté "métier".
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email required", nameof(email));
            
            if (!IsValidEmail(email))
                throw new ArgumentException("Email is invalid", nameof(email));
            
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password required", nameof(password));
            
            if (password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters", nameof(password));
            
            var user = new User(email, password);
            
            return _userRepository.Insert(user);
        }

        public User? Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            var user = _userRepository.CheckPassword(email, password);
            
            // ici on peut ajouter des règles métiers :
            // - Vérifier que le compte est actif avec (isActive)

            if (user == null)
                return null;
            
            if (!user.isActive)
                return null;

            return user;
        }

        public void Disable(Guid id)
        {
            _userRepository.Disable(id);
        }
        
        #region Private methdos
    
        /// <summary>
        /// Validation en plus de la validation côté asp assez Simple pour la démo
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    
        #endregion
    }
}