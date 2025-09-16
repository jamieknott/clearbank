using ClearBank.DeveloperTest.Interfaces;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Factories
{
    public interface IPaymentSchemeFactory
    {
        public IPaymentScheme GetScheme(PaymentScheme scheme);
    }
}