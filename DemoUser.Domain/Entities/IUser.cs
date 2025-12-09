using System;
using System.Collections.Generic;
using System.Text;

namespace DemoUser.Domain.Entities
{
    public interface IUser
    {
        public Guid Id { get; }
        public string Email { get; }
        public string Password { get; }
        public DateTime CreatedAt { get;  }
        public DateTime? DisabledAt { get; }
    }
}
