using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Interfaces
{
    public interface IDataService
    {
        public Account GetAccount(string accountNumber);
        public void UpdateAccount(Account account);
    }
}