using MojaBiblioteka.Data.SQLite;
using MojaBiblioteka.Utility;
using System;
using System.Data.SQLite;

namespace MojaBiblioteka.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository()
        {
            var dbDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MojaBiblioteka");
            var databasePath = Path.Combine(dbDirectory, "moja-biblioteka.sqlite");

            DataBaseInitializer.Initialize(databasePath);
            _connectionString = $"Data Source={databasePath};Version=3;";
        }

        public bool Exists(string login)
        {
            const string sql = @"
SELECT 
    COUNT(1) 
FROM 
    Users 
WHERE 
    Login = @Login;";
            using (var connection = CreateOpenConnection())
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Login", login);
                var count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        public UserModel GetByCredentials(string login, string passwordHash)
        {
            const string sql = @"
SELECT 
    Id, 
    Login, 
    PasswordHash
FROM
    Users
WHERE 
    Login = @Login 
    AND PasswordHash = @PasswordHash
LIMIT 1;";

            using (var connection = CreateOpenConnection())
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    return new UserModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Login = reader["Login"]?.ToString(),
                        PasswordHash = reader["PasswordHash"]?.ToString()
                    };
                }
            }
        }

        public UserModel Add(string login, string passwordHash)
        {
            const string sql = @"
INSERT INTO Users (
    Login, 
    PasswordHash)
VALUES (
    @Login, 
    @PasswordHash);
SELECT 
    last_insert_rowid();";

            using (var connection = CreateOpenConnection())
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                var id = Convert.ToInt32(command.ExecuteScalar());

                return new UserModel
                {
                    Id = id,
                    Login = login,
                    PasswordHash = passwordHash
                };
            }
        }

        private SQLiteConnection CreateOpenConnection()
        {
            var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
