using ModelLibrary;
using ModelLibrary.DatabaseTables;
using SQLite;

namespace API_Playground.Services
{
    public class DatabaseService
    {
        private static string rootDirectory = Directory.GetCurrentDirectory();
        public static string DatabasePath { get; } = Path.Combine(rootDirectory, "db" , "local_storage.db");

        public DatabaseService()
        {
            if (!File.Exists(DatabasePath))
            {
                using (var db = new SQLiteConnection(DatabasePath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite))
                {
                    db.EnableWriteAheadLogging();
                    db.ExecuteScalar<string>("PRAGMA foreign_keys=ON;");
                    db.ExecuteScalar<string>("PRAGMA journal_mode=WAL;");
                    db.ExecuteScalar<string>("PRAGMA synchronous=OFF;");
                }

                ProductTable productTable = new(DatabasePath);

                Thread.Sleep(1000);

                using (var db = new SQLiteConnection(DatabasePath, SQLiteOpenFlags.ReadWrite))
                {
                    string insertQuery = "INSERT INTO Product (Name, Price, Description) VALUES (?, ?, ?)";

                    List<Product> list = new List<Product>
                        {
                            new("Item 1", 100),
                            new("Item 2", 200),
                            new("Item 3", 300),
                            new("Item 4", 400),
                            new("Item 5", 500),
                            new("Item 6", 600)
                        };

                    foreach (Product item in list)
                    {
                        db.Execute(insertQuery, item.Name, item.Price, item.Description);
                    }
                }
            }
        }
    }
}
