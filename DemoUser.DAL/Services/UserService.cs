using DemoUser.DAL.Entities;
using DemoUser.DAL.Mapper;
using DemoUser.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace DemoUser.DAL.Services
{
    public class UserService : IUserRepository<User>
    {
        private DbConnection _connection;

        public UserService(DbConnection connection)
        {
            _connection = connection;
        }

        public Guid CheckPassword(string email, string password)
        {
            Guid id;
            using(DbCommand command = _connection.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "SP_User_CheckPassword";
                AddParam(command, nameof(email), email);
                AddParam(command, nameof(password), password);
                _connection.Open();
                id = (Guid)command.ExecuteScalar();
                _connection.Close();
            }
            return id;
        }

        public void Delete(Guid id)
        {
            using (DbCommand command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM [User] WHERE [Id] = @id";
                AddParam(command, nameof(id), id);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public IEnumerable<User> Get()
        {
            throw new NotImplementedException();
        }

        public User Get(Guid id)
        {
            User entity;
            using (DbCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT [Id], [Email], [CreatedAt], [DisabledAt] FROM [User] WHERE [Id] = @id";
                AddParam(command, nameof(id), id);
                _connection.Open();
                using(DbDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        entity = reader.ToDAL();
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException(nameof(id));
                    }
                }

                _connection.Close();

            }
            return entity;
        }

        public Guid Insert(User entity)
        {
            using (DbCommand command = _connection.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "SP_User_Insert";
                AddParam(command, nameof(entity.Email), entity.Email);
                AddParam(command, nameof(entity.Password), entity.Password);
                _connection.Open();
                entity.Id = (Guid)command.ExecuteScalar();
                _connection.Close();
            }
            return entity.Id;
        }

        public void Update(Guid id, User entity)
        {
            throw new NotImplementedException();
        }

        private void AddParam(DbCommand command, string paramName, object? paramValue)
        {
            DbParameter param = command.CreateParameter();
            param.ParameterName = paramName;
            param.Value = paramValue;
            command.Parameters.Add(param);
        }
    }
}
