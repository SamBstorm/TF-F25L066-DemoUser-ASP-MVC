using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DemoUser.DAL.Mappers;
using DemoUser.Domain.Entities;
using DemoUser.Domain.Repositories;
using Microsoft.Data.SqlClient;

namespace DemoUser.DAL.Repositories
{

    public class SqlUserRepository : IUserRepository<User>
    {
        private readonly string _connectionString;

        public SqlUserRepository(string  connectionString)
        {
            _connectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
        }
        
        #region Private methods
        
        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);
        
        #endregion

        public IEnumerable<User> Get()
        {
            var users = new List<User>();
            
            using var connection = CreateConnection();
            using var command = connection.CreateCommand();
            
            command.CommandText = "SP_User_GetAll";
            command.CommandType = CommandType.StoredProcedure;
            
            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                users.Add(Mapper.MapUser(reader));
            }

            return users;
        }

        public User? Get(Guid id)
        {
            using var connection = CreateConnection();
            using var command = connection.CreateCommand();
            
            command.CommandText = "SP_User_GetById";
            command.CommandType = CommandType.StoredProcedure;
            
            command.Parameters.AddWithValue("@Id", id);
            
            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.Read()) // Si jamais il y a bien un utilisateur avec cet id
            {
                return Mapper.MapUser(reader);
            }
            
            return null;
        }

        /// <summary>
        /// J'ai un insert avec un id, à partir du moment où mon insert prend en compte le fait que lors d'un insert
        /// l'utilisateur existe déjà et donc qu'il va non pas créer mais modifier un utilisateur.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Guid Insert(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));
            
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Email required", nameof(user.Email));
            
            if (string.IsNullOrWhiteSpace(user.Password))
                throw new ArgumentException("Password required", nameof(user.Password));
            
            using var connection = CreateConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "SP_User_Insert";
            command.CommandType = CommandType.StoredProcedure;
            
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);

            connection.Open();
            var result = command.ExecuteScalar();
            
            return (Guid)result;
        }

        public User? CheckPassword(string email, string password)
        {
            using var connection = CreateConnection();
            using var command = connection.CreateCommand();
            
            command.CommandText = "SP_User_CheckPassword";
            command.CommandType = CommandType.StoredProcedure;
            
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", password);
            
            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return Mapper.MapUser(reader);
            }
            
            return null;
        }

        public void Update(Guid id, User entity)
        {
            throw new NotImplementedException(nameof(Update));
        }

        public void Disable(Guid id)
        {
            using var connection = CreateConnection();
            using var command = connection.CreateCommand();
            
            command.CommandText = "SP_User_Disable";
            command.CommandType = CommandType.StoredProcedure;
            
            command.Parameters.AddWithValue("@Id", id);
            
            connection.Open();
            command.ExecuteNonQuery();
        }

        
        
    }
}
