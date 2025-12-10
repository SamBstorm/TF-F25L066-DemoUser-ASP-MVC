using System;
using System.Collections.Generic;
using DemoUser.Domain.Entities;

namespace DemoUser.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Session CreateForUser(Guid userId, TimeSpan lifetime);
        Session? GetByToken(Guid token);
        IEnumerable<Session> GetForUser(Guid userId);
        void Revoke(Guid id);
    }
}