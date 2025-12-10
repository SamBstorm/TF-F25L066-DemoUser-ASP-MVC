using System;

namespace DemoUser.Domain.Entities
{
    public class User
    {
        public Guid Id { get; }
        public string Email { get; }
        public string Password { get; }
        public DateTime CreatedAt { get; }
        public DateTime? DisabledAt { get; }
        public bool isActive => DisabledAt == null || DisabledAt > DateTime.UtcNow;

        #region Constructeurs

        //Ctor utilisé quand on hydrate depuis la base de données
        public User(
            Guid id,
            string email,
            DateTime createdAt,
            DateTime? disabledAt
            )
        {
            Id = id;
            Email = email;
            CreatedAt = createdAt;
            DisabledAt = disabledAt;
        }
        
        // Ctor utilisé quand on crée un nouvel utilisateur côté BLL
        public User(string email, string password)
        {
            Id = Guid.Empty; // c'est la responsabilité de la DB de définir l'id.
            Email = email;
            Password = password;
            CreatedAt = DateTime.UtcNow;
            DisabledAt = null;
        }

        #endregion
    }
}
