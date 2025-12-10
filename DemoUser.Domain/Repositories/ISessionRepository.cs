using DemoUser.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace DemoUser.Domain.Repositories
{
    public interface ISessionRepository
    {
        Session Insert(Session session);
        Session? GetByToken(Guid token);
        IEnumerable<Session> GetByUser(Guid userId);
        void Revoke(Guid id);
    }
}
