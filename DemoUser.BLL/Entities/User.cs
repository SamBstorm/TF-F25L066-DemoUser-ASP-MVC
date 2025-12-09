using DemoUser.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoUser.BLL.Entities
{
    public class User : IUser
    {

        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public int DaysSinceCreation
        {
            get {
                return (DateTime.Now - CreatedAt).Days;
            }
        }
        public DateTime? DisabledAt { get; private set; }

        public User(Guid id, string email, string password, DateTime createdAt, DateTime? disabledAt) : this(email, password)
        {
            Id = id;
            CreatedAt = createdAt;
            DisabledAt = disabledAt;
        }

        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }


    }
}
