using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Interfaces
{
    public interface IPaymentScheme
    {
        public bool IsValid(Account account, MakePaymentRequest request);
    }
}
