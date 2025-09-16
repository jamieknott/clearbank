using ClearBank.DeveloperTest.Interfaces;
using ClearBank.DeveloperTest.Schemes;
using ClearBank.DeveloperTest.Types;
using System;

namespace ClearBank.DeveloperTest.Factories
{
    public class PaymentSchemeFactory : IPaymentSchemeFactory
    {
        public IPaymentScheme GetScheme(PaymentScheme scheme) => scheme switch
        {
            PaymentScheme.Bacs => new BacsPaymentScheme(),
            _ => throw new NotSupportedException($"Unsupported payment scheme: {scheme}")
        };
    }
}
