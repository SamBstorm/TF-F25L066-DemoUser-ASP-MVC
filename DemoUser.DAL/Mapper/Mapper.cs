using DemoUser.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DemoUser.DAL.Mapper
{
    internal static class Mapper
    {
        public static User ToDAL(this IDataRecord record)
        {
            if (record is null) throw new ArgumentNullException(nameof(record));
            return new User()
            {
                Id = (Guid)record[nameof(User.Id)],
                Email = (string)record[nameof(User.Email)],
                CreatedAt = (DateTime)record[nameof(User.CreatedAt)],
                DisabledAt = (record[nameof(User.DisabledAt)] is DBNull) ? null : (DateTime?)record[nameof(User.DisabledAt)],
                Password = "********"
            };
        }
    }
}
