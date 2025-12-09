using System;
using System.Collections.Generic;
using System.Text;

namespace DemoUser.BLL.Mapper
{
    internal static class Mapper
    {
        public static BLL.Entities.User ToBLL(this DAL.Entities.User entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            return new BLL.Entities.User(
                entity.Id,
                entity.Email,
                entity.Password,
                entity.CreatedAt,
                entity.DisabledAt);
        }
        public static DAL.Entities.User ToDAL(this BLL.Entities.User entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            return new DAL.Entities.User()
            {
                Id= entity.Id,
                Email = entity.Email,
                Password = entity.Password,
                CreatedAt = entity.CreatedAt,
                DisabledAt = entity.DisabledAt
            };
        }
    }
}
