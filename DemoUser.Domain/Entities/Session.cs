using System;

namespace DemoUser.Domain.Entities
{
    public class Session
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public Guid Token { get; }
        public DateTime CreatedAt { get; }
        public DateTime ExpiresAt { get; }
        public DateTime? RevokedAt { get; }
        public bool isActive => RevokedAt == null && RevokedAt > DateTime.UtcNow;

        #region Constructeurs

        //Ctor utilisé quand on hydrate depuis la base de données
        public Session(
            Guid id,
            Guid userId,
            Guid token,
            DateTime createdAt,
            DateTime expiresAt,
            DateTime? revokedAt
            )
        {
            Id = id;
            UserId = userId;
            Token = token;
            CreatedAt = createdAt;
            ExpiresAt = expiresAt;
            RevokedAt = revokedAt;
        }
        
        // Ctor côté création BLL
        public Session(Guid userId, TimeSpan lifetime)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Token = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            ExpiresAt = CreatedAt.Add(lifetime);
        }

        #endregion
    }
}
