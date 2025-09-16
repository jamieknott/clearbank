using ClearBank.DeveloperTest.Interfaces;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Schemes
{
    public class BacsPaymentScheme : IPaymentScheme
    {
        public bool IsValid(Account account, MakePaymentRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
