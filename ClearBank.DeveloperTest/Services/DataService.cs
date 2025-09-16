using ClearBank.DeveloperTest.Interfaces;
using ClearBank.DeveloperTest.Types;
using System.Configuration;

namespace ClearBank.DeveloperTest.Services
{
    public class DataService : IDataService
    {
        private readonly IAccountDataStore _primaryAccountDataStore;
        private readonly IAccountDataStore _backupAccountDataStore;

        private readonly string _dataStoreType;

        public DataService(IAccountDataStore primaryAccountDataStore, IAccountDataStore backupAccountDataStore)
        {
            _primaryAccountDataStore = primaryAccountDataStore; // configure primary data store connection
            _backupAccountDataStore = backupAccountDataStore; // configure backup data store connection

            _dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];
        }

        // Followed initial setup here but would rather rely on a backup as a fallback instead of toggling through config.
        private IAccountDataStore DataStore =>
            _dataStoreType == "Backup" ? _backupAccountDataStore : _primaryAccountDataStore;

        public Account GetAccount(string accountNumber) => DataStore.GetAccount(accountNumber);

        public void UpdateAccount(Account account) => DataStore.UpdateAccount(account);
    }
}
