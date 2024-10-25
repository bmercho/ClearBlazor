using TestData;
using Microsoft.EntityFrameworkCore;

namespace ListsTest
{
    public enum DatabaseType
    {
        SQLite,
        SqlServer
    }

    public class DatabaseManager
    {

        private DatabaseType _databaseType = DatabaseType.SQLite;
        private string _databaseName = string.Empty;
        private string _databaseHost = string.Empty;
        private string _password = string.Empty;
        private string _userName = string.Empty;
        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);


        public void Start(DatabaseType databaseType, string databaseName, 
                          string databaseHost, string userName, string password)
        {
            _databaseType = databaseType;
            _databaseName = databaseName;
            _databaseHost = databaseHost;
            _userName = userName;
            _password = password;

            TestDbContext.Initialise(_databaseType, _databaseName, _databaseHost, _userName, _password);

            if (!IsDbCreated())
            {
                CreateDb();
                AddTestData();
            }
        }

        private void AddTestData()
        {
            using (var dbContext = new TestDbContext())
            {
                try
                {
                    for (int i = 0; i < 5000; i++)
                    {
                        dbContext.Feeds.Add(FeedEntry.GetNewFeed(i));
                        dbContext.SaveChanges();
                    }


                    for (int i = 0; i < 5000; i++)
                        dbContext.TableRows.Add(TableRow.GetNewTableRow(i));
                    dbContext.SaveChanges();
                }
                catch(Exception ex)
                {
                    return;
                }
            }

        }

        private bool IsDbCreated()
        {
            using (var dbContext = new TestDbContext())
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
            using (var dbContext = new TestDbContext())
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
            await semaphoreSlim.WaitAsync();
            try
            {
                FeedEntryResult result = new FeedEntryResult();
                using (var dbContext = new TestDbContext())
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
            finally 
            { 
                semaphoreSlim.Release(); 
            }
        }
        public async Task<TableRowResult> GetTablesRows(int firstIndex, int count)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                TableRowResult result = new ();
                using (var dbContext = new TestDbContext())
                {
                    try
                    {
                        result.TotalNumEntries = dbContext.TableRows.Count();
                        result.FirstIndex = firstIndex;
                        result.TableRows = await dbContext.TableRows.OrderBy(r => r.Index).Skip(firstIndex).Take(count).ToListAsync();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return result;
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

    }
}
