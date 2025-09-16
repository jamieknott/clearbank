using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Interfaces
{
    public interface IAccountDataStore
    {
        public Account GetAccount(string accountNumber);
        public void UpdateAccount(Account account);
    }
}
