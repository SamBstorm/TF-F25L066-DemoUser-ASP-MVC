using System;
using System.Collections.Generic;
using DemoUser.BLL.Services.Interfaces;
using DemoUser.Domain.Entities;
using DemoUser.Domain.Repositories;

namespace DemoUser.BLL.Services.Implementations
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionService(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public Session CreateForUser(Guid userId, TimeSpan lifetime)
            => _sessionRepository.Insert(new Session(userId, lifetime));
        
        public Session? GetByToken(Guid token)
            => _sessionRepository.GetByToken(token);
        
        public IEnumerable<Session> GetForUser(Guid userId)
            => _sessionRepository.GetByUser(userId);

        public void Revoke(Guid id)
            => _sessionRepository.Revoke(id);
    }
}