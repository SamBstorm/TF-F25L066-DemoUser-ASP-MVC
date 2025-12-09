using System;
using System.Collections.Generic;
using System.Text;

namespace DemoUser.Domain.Repositories
{
    public interface ICRUDRepository<TEntity, TId>
    {
        IEnumerable<TEntity> Get();
        TEntity Get(TId id);
        TId Insert(TEntity entity);
        void Update(TId id, TEntity entity);
        void Delete(TId id);
    }
}
