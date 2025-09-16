using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Interfaces
{
    public interface IPaymentSchemeFactory
    {
        public IPaymentScheme GetScheme(PaymentScheme scheme);
    }
}