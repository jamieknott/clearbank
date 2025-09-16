using ClearBank.DeveloperTest.Interfaces;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Schemes
{
    public class FasterPaymentsScheme : IPaymentScheme
    {
        public bool IsValid(Account account, MakePaymentRequest request) =>
            account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) &&
            account.Balance > request.Amount;
    }
}
