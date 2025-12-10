using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DemoUser.Domain.Entities;
using DemoUser.Domain.Repositories;
using Microsoft.Data.SqlClient;

namespace DemoUser.DAL.Repositories
{
    public class SqlSessionRepository: ISessionRepository
    {
        private readonly string _connectionString;

        public SqlSessionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private DbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
        
        public Session Insert(Session session)
        {
            using var connection = CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = @"
                INSERT INTO [Session] (Id, UserId, Token, CreatedAt, ExpiresAt, RevokedAt)
                VALUES (@Id, @UserId, @Token, @CreatedAt, @ExpiresAt, @RevokedAt);

                SELECT Id, UserId, Token, CreatedAt, ExpiresAt, RevokedAt
                FROM [Session]
                WHERE Id = @Id;
            ";

            AddParameter(command, "@Id", session.Id);
            AddParameter(command, "@UserId", session.UserId);
            AddParameter(command, "@Token", session.Token);
            AddParameter(command, "@CreatedAt", session.CreatedAt);
            AddParameter(command, "@ExpiresAt", session.ExpiresAt);
            AddParameter(command, "@RevokedAt", (object?)session.RevokedAt ?? DBNull.Value);

            using var reader = command.ExecuteReader();
            if (!reader.Read()) throw new InvalidOperationException("Failed to insert session.");

            return MapSession(reader);
        }
        
        public Session? GetByToken(Guid token)
        {
            using var connection = CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = @"
                SELECT Id, UserId, Token, CreatedAt, ExpiresAt, RevokedAt
                FROM [Session]
                WHERE Token = @Token;
            ";

            AddParameter(command, "@Token", token);

            using var reader = command.ExecuteReader();
            
            if (!reader.Read()) return null;

            return MapSession(reader);
        }
        
        public IEnumerable<Session> GetByUser(Guid userId)
        {
            var list = new List<Session>();

            using var connection = CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = @"
                SELECT Id, UserId, Token, CreatedAt, ExpiresAt, RevokedAt
                FROM [Session]
                WHERE UserId = @UserId
                ORDER BY CreatedAt DESC;
            ";

            AddParameter(command, "@UserId", userId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(MapSession(reader));
            }

            return list;
        }
        
        /// <summary>
        /// Avec ce qu'on fait ici avec le revoke, il faut comprendre que dans la réalite,
        /// on lance une action automatique tous les X temps qui nettoie les tokens revoqués.
        ///
        /// Exemple, la nuit, mon app a peu d'utilisateur, je lance l'action automatique qui fait
        /// une requête sql et qui nettoie tous les revoked depuis xTemps.
        /// </summary>
        /// <param name="id"></param>
        public void Revoke(Guid id)
        {
            using var connection = CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = @"
                UPDATE [Session]
                SET RevokedAt = GETDATE()
                WHERE Id = @Id;
            ";

            AddParameter(command, "@Id", id);
            command.ExecuteNonQuery();
        }
        
        #region Private methods

        private static Session MapSession(IDataRecord record)
        {
            return new Session(
                id: (Guid)record["Id"],
                userId:  (Guid)record["UserId"],
                token:  (Guid)record["Token"],
                createdAt:  (DateTime)record["CreatedAt"],
                expiresAt:  (DateTime)record["ExpiresAt"],
                revokedAt: record["RevokedAt"] == DBNull.Value
                    ? null
                    : (DateTime?)record["RevokedAt"]
            );
        }

        private static void AddParameter(DbCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }
        
        #endregion
    }
}