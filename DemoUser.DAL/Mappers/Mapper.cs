using System;
using System.Data;
using DemoUser.Domain.Entities;

namespace DemoUser.DAL.Mappers
{
    public static class Mapper
    {
        public static User MapUser(IDataRecord record)
        {
            return new User(
                id: (Guid)record["Id"],
                email:(string)record["Email"],
                createdAt:(DateTime)record["CreatedAt"],
                disabledAt: record["DisabledAt"] is DBNull
                    ? null
                    : (DateTime?)record["DisabledAt"]
            );
        }
    }
}
