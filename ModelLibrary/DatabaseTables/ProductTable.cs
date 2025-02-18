using SQLite;

namespace ModelLibrary.DatabaseTables
{
    public class ProductTable
    {
        public ProductTable(string databasePath)
        {
            using (var db = new SQLiteConnection(databasePath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite))
            {
                db.Execute(@$"CREATE TABLE IF NOT EXISTS Product(
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name NVARCHAR(50) NOT NULL,
                            Price DOUBLE NOT NULL,
                            Description NVARCHAR(800)
                            );");
            }
        }
    }
}
