using System.Data.SQLite;
using System.IO;

namespace MojaBiblioteka.Data.SQLite
{
    public static class DataBaseInitializer
    {
        public static readonly string CreateTablesIfNeeded = @"
CREATE TABLE IF NOT EXISTS Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Login TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Books (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER NOT NULL,
    Title TEXT NOT NULL,
    Author TEXT NOT NULL,
    Isbn TEXT NULL,
    Year INTEGER NULL,
    Genre TEXT NULL,
    Description TEXT NULL,
    CoverImage BLOB NULL,
    FOREIGN KEY(UserId) REFERENCES Users(Id)
);";

        public static void Initialize(string databasePath)
        {
            var directory = Path.GetDirectoryName(databasePath);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(databasePath))
            {
                SQLiteConnection.CreateFile(databasePath);
            }

            using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = CreateTablesIfNeeded;
                    command.ExecuteNonQuery();
                }

                EnsureBooksUserIdColumn(connection);
            }
        }

        private static void EnsureBooksUserIdColumn(SQLiteConnection connection)
        {
            const string pragmaSql = "PRAGMA table_info(Books);";
            using (var command = new SQLiteCommand(pragmaSql, connection))
            using (var reader = command.ExecuteReader())
            {
                var hasUserId = false;
                while (reader.Read())
                {
                    if ((reader["name"]?.ToString() ?? string.Empty).Equals("UserId"))
                    {
                        hasUserId = true;
                        break;
                    }
                }

                if (hasUserId)
                {
                    return;
                }
            }

            using (var alterCommand = new SQLiteCommand("ALTER TABLE Books ADD COLUMN UserId INTEGER NOT NULL DEFAULT 0;", connection))
            {
                alterCommand.ExecuteNonQuery();
            }
        }
    }
}
