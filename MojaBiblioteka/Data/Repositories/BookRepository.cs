using MojaBiblioteka.Data.SQLite;
using MojaBiblioteka.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace MojaBiblioteka.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString;

        public BookRepository(string databasePath)
        {
            DataBaseInitializer.Initialize(databasePath);
            _connectionString = $"Data Source={databasePath};Version=3;";
        }

        public IEnumerable<BookModel> GetAll(int userId)
        {
            const string sql = @"
SELECT
    Id,
    UserId,
    Title,
    Author,
    Year,
    Genre,
    Description,
    CoverImage
FROM 
    Books
WHERE 
    UserId = @UserId
ORDER BY 
    Title;";

            var books = new List<BookModel>();

            using (var connection = CreateOpenConnection())
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new BookModel
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            UserId = GetNullableInt(reader, "UserId"),
                            Title = reader["Title"]?.ToString(),
                            Author = reader["Author"]?.ToString(),
                            Year = GetNullableInt(reader, "Year"),
                            Genre = GetNullableString(reader, "Genre"),
                            Description = GetNullableString(reader, "Description"),
                            CoverImageData = GetNullableBlob(reader, "CoverImage")
                        });
                    }
                }
            }

            return books;
        }

        public BookModel Add(BookModel book)
        {
            const string sql = @"
INSERT INTO Books (
    UserId, 
    Title, 
    Author, 
    Year, 
    Genre, 
    Description, 
    CoverImage)
VALUES (
    @UserId, 
    @Title, 
    @Author, 
    @Year, 
    @Genre, 
    @Description, 
    @CoverImage);
SELECT 
    last_insert_rowid();";

            var cover = ResolveCoverBytes(book);

            using (var connection = CreateOpenConnection())
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@UserId", book.UserId);
                command.Parameters.AddWithValue("@Title", book.Title ?? string.Empty);
                command.Parameters.AddWithValue("@Author", book.Author ?? string.Empty);
                command.Parameters.AddWithValue("@Year", book.Year > 0 ? (object)book.Year : DBNull.Value);
                command.Parameters.AddWithValue("@Genre", (object)book.Genre ?? DBNull.Value);
                command.Parameters.AddWithValue("@Description", (object)book.Description ?? DBNull.Value);
                command.Parameters.Add("@CoverImage", DbType.Binary).Value = (object)cover ?? DBNull.Value;

                var insertedId = Convert.ToInt32(command.ExecuteScalar());
                book.Id = insertedId;
                book.CoverImageData = cover;
            }

            return book;
        }

        public void Update(BookModel book)
        {
            const string sql = @"
UPDATE 
    Books
SET
    Title = @Title,
    Author = @Author,
    Year = @Year,
    Genre = @Genre,
    Description = @Description,
    CoverImage = @CoverImage
WHERE 
    Id = @Id 
    AND UserId = @UserId;";

            var cover = ResolveCoverBytes(book);

            using (var connection = CreateOpenConnection())
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", book.Id);
                command.Parameters.AddWithValue("@UserId", book.UserId);
                command.Parameters.AddWithValue("@Title", book.Title ?? string.Empty);
                command.Parameters.AddWithValue("@Author", book.Author ?? string.Empty);
                command.Parameters.AddWithValue("@Year", book.Year > 0 ? (object)book.Year : DBNull.Value);
                command.Parameters.AddWithValue("@Genre", (object)book.Genre ?? DBNull.Value);
                command.Parameters.AddWithValue("@Description", (object)book.Description ?? DBNull.Value);
                command.Parameters.Add("@CoverImage", DbType.Binary).Value = (object)cover ?? DBNull.Value;
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id, int userId)
        {
            const string sql = "DELETE FROM Books WHERE Id = @Id AND UserId = @UserId;";

            using (var connection = CreateOpenConnection())
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
            }
        }

        private SQLiteConnection CreateOpenConnection()
        {
            var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            return connection;
        }

        private static byte[] ResolveCoverBytes(BookModel book)
        {
            if (book.CoverImageData != null && book.CoverImageData.Length > 0)
            {
                return book.CoverImageData;
            }

            if (!string.IsNullOrWhiteSpace(book.CoverImagePath) && File.Exists(book.CoverImagePath))
            {
                return File.ReadAllBytes(book.CoverImagePath);
            }

            return null;
        }

        private static string GetNullableString(SQLiteDataReader reader, string columnName) =>
            reader[columnName] == DBNull.Value ? null : reader[columnName]?.ToString();

        private static int GetNullableInt(SQLiteDataReader reader, string columnName) =>
            reader[columnName] == DBNull.Value ? 0 : Convert.ToInt32(reader[columnName]);

        private static byte[] GetNullableBlob(SQLiteDataReader reader, string columnName) =>
            reader[columnName] == DBNull.Value ? null : (byte[])reader[columnName];
    }
}
