using System;
using System.Collections.Generic;
using System.Text;

namespace DemoUser.Domain.Entities
{
    public interface IUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DisabledAt { get; set; }
    }
}
