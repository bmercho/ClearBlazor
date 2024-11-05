using Microsoft.EntityFrameworkCore;
using Data;

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
                    var listData = Data.TestData.GetTestListRows(5000);
                    foreach (var row in listData)
                        dbContext.TestListRows.Add(row);
                    dbContext.SaveChanges();

                    var treeData = Data.TestData.GetTestTreeRows(5000);
                    var treeDataFlat = Data.TestData.GetTestTreeRowsFlat(treeData);

                    foreach (var row in treeDataFlat)
                        dbContext.TestTreeRows.Add(row);
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
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

        public async Task<ListEntryResult> GetListRows(int firstIndex, int count)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                ListEntryResult result = new ListEntryResult();
                using (var dbContext = new TestDbContext())
                {
                    try
                    {
                        result.TotalNumEntries = dbContext.TestListRows.Count();
                        result.FirstIndex = firstIndex;
                        result.ListRows = await dbContext.TestListRows.Skip(firstIndex).Take(count).ToListAsync();
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
        public async Task<TreeEntryResult> GetTreeRows(int firstIndex, int count)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                TreeEntryResult result = new();
                using (var dbContext = new TestDbContext())
                {
                    try
                    {
                        result.TotalNumEntries = dbContext.TestTreeRows.Count();
                        result.FirstIndex = firstIndex;
                        result.TreeRows = await dbContext.TestTreeRows.
                                         OrderBy(r => r.Index).Skip(firstIndex).
                                         Take(count).ToListAsync();
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
