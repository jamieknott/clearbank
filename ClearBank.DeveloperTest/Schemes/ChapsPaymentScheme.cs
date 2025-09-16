using ClearBank.DeveloperTest.Interfaces;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Schemes
{
    public class ChapsPaymentScheme : IPaymentScheme
    {
        public bool IsValid(Account account, MakePaymentRequest request) =>
            account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) &&
            account.Status == AccountStatus.Live;
    }
}
