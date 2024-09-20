using Microsoft.EntityFrameworkCore;

namespace VirtualizeDemo
{
    public class DatabaseManager
    {
        public enum DatabaseType
        {
            SQLite,
            SqlServer
        }

        private DatabaseType _databaseType = DatabaseType.SQLite;
        private string _databaseName = string.Empty;
        private string _databaseHost = string.Empty;
        private string _password = string.Empty;
        private string _userName = string.Empty;

        public void Start(DatabaseType databaseType, string databaseName, 
                          string databaseHost, string userName, string password)
        {
            _databaseType = databaseType;
            _databaseName = databaseName;
            _databaseHost = databaseHost;
            _userName = userName;
            _password = password;

            FeedContext.Initialise(_databaseType, _databaseName, _databaseHost, _userName, _password);

            if (!IsDbCreated())
            {
                CreateDb();
                AddTestData();
            }
        }

        private void AddTestData()
        {
            using (var dbContext = new FeedContext())
            {
                try
                {
                    var user1 = dbContext.Users.Add(new User() { UserName="User1" });
                    var user2 = dbContext.Users.Add(new User() { UserName = "User2" });
                    var user3 = dbContext.Users.Add(new User() { UserName = "User3" });
                    var user4 = dbContext.Users.Add(new User() { UserName = "User4" });
                    var user5 = dbContext.Users.Add(new User() { UserName = "User5" });

                    for (int i = 0; i < 500; i++)
                    {
                        dbContext.Feeds.Add(new FeedEntry()
                        {
                            EntryType = FeedEntryType.TextOnly,
                            Title = $"Message{i + 1}",
                            Message = LoremNET.Lorem.Words(10, 100),
                            PosterUserId = user1.Entity.UserId,
                            ElementId = i,
                            TimeStamp = DateTime.Now
                        });
                        dbContext.SaveChanges();
                    }

                }
                catch
                {
                    return;
                }
            }

        }

        private bool IsDbCreated()
        {
            using (var dbContext = new FeedContext())
            {
                try
                {
                    return dbContext.Database.CanConnect();
                }
                catch
                {
                    return false;
                }
            }
        }

        private string CreateDb()
        {
            using (var dbContext = new FeedContext())
            {
                try
                {
                    dbContext.Database.EnsureCreated();
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        public async Task<FeedEntryResult> GetFeeds(int firstIndex, int count)
        {
            FeedEntryResult result = new FeedEntryResult();
            using (var dbContext = new FeedContext())
            {
                try
                {
                    result.TotalNumEntries = dbContext.Feeds.Count();
                    result.FirstIndex = firstIndex;
                    result.FeedEntries = await dbContext.Feeds.Skip(firstIndex).Take(count).ToListAsync();
                }
                catch (Exception ex)
                {
                }
            }
            return result;
        }

    }
}
