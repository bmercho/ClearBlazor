using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ListsTest
{
    public class TestDbContext:DbContext
    {
        public DbSet<FeedEntry> Feeds { get; set; }
        public DbSet<TableRow> TableRows { get; set; }

        private static DatabaseType _databaseType = DatabaseType.SQLite;
        private static string _databaseName = string.Empty;
        private static string _databaseHost = string.Empty;
        private static string _password = string.Empty;
        private static string _userName = string.Empty;
        private static string _instanceName = string.Empty;
        private static string _userDomain = string.Empty;

        public static void Initialise(DatabaseType databaseType, string databaseName, 
                                      string databaseHost, string userName, string password)
        {
            _databaseType = databaseType;
            _databaseName = databaseName;
            _databaseHost = databaseHost;
            _userName = userName;
            _password = password;
        }

        public TestDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.EnableSensitiveDataLogging();
            if (_databaseType == DatabaseType.SQLite)
                options.UseSqlite(BuildConnectionString());
            else
                options.UseSqlServer(BuildConnectionString());
        }
        private string BuildConnectionString()
        {
            var sb = new StringBuilder();

            if (_databaseType == DatabaseType.SQLite)
            {
                sb.AppendFormat($"Data Source={_databaseName}");
            }
            else
            {
                if (string.IsNullOrEmpty(_instanceName))
                {
                    sb.AppendFormat("Data Source={0};", _databaseHost);
                }
                else
                {
                    sb.AppendFormat("Data Source={0}\\{1};", _databaseHost, _instanceName);
                }

                sb.AppendFormat("Initial Catalog={0};", _databaseName);

                if (string.IsNullOrEmpty(_userDomain))
                {
                    sb.AppendFormat("User ID={0};", _userName);
                }
                else
                {
                    sb.AppendFormat("User ID={0}\\{1};", _userDomain, _userName);
                }

                sb.AppendFormat("Password={0};", _password);

                sb.Append("MultipleActiveResultSets=True;");
                sb.Append("Encrypt=False;");
            }
            return sb.ToString();
        }

    }
}
