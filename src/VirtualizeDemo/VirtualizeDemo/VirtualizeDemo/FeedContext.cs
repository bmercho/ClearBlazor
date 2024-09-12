using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;
using System.Text;
using static VirtualizeDemo.DatabaseManager;

namespace VirtualizeDemo
{
    public class FeedContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<FeedEntry> Feeds { get; set; }

        public string DbPath { get; }

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

        public FeedContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "feeds.db");
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
