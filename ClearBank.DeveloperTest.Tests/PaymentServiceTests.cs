using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        private readonly PaymentService _service;

        public PaymentServiceTests()
        {
            _service = new PaymentService();
        }

        [Theory]
        [InlineData(PaymentScheme.Bacs, true)]
        [InlineData(PaymentScheme.Chaps, false)]
        public void MakePayment_BacsScheme_Validation_ReturnsExpected(PaymentScheme requestPaymentScheme, bool expected)
        {
            // Act
            var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs };
            /// unable to mock account until we decouple the data store from the payment service

            var request = new MakePaymentRequest { DebtorAccountNumber = "123", PaymentScheme = requestPaymentScheme };

            // Arrange
            var result = _service.MakePayment(request);

            // Assert
            Assert.Equal(result.Success, expected);
        }

        [Theory]
        [InlineData(PaymentScheme.Chaps, AccountStatus.Live, true)]
        [InlineData(PaymentScheme.Chaps, AccountStatus.InboundPaymentsOnly, false)]
        [InlineData(PaymentScheme.Bacs, AccountStatus.Live, false)]
        [InlineData(PaymentScheme.FasterPayments, AccountStatus.Disabled, false)]
        public void MakePayment_ChapsScheme_Validation_ReturnsExpected(PaymentScheme requestPaymentScheme, AccountStatus status, bool expected)
        {
            // Act
            var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Status = status };
            /// unable to mock account until we decouple the data store from the payment service

            var request = new MakePaymentRequest { DebtorAccountNumber = "123", PaymentScheme = requestPaymentScheme };

            // Arrange
            var result = _service.MakePayment(request);

            // Assert
            Assert.Equal(result.Success, expected);
        }

        [Theory]
        [InlineData(PaymentScheme.FasterPayments, 10, 8, true)]
        [InlineData(PaymentScheme.FasterPayments, 10, 15, false)]
        [InlineData(PaymentScheme.Bacs, 10, 8, false)]
        public void MakePayment_FasterPaymentsScheme_Validation_ReturnsExpected(PaymentScheme requestPaymentScheme, int balance, int requestAmount, bool expected)
        {
            // Act
            var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Balance = balance };
            /// unable to mock account until we decouple the data store from the payment service

            var request = new MakePaymentRequest { DebtorAccountNumber = "123", PaymentScheme = requestPaymentScheme, Amount = requestAmount };

            // Arrange
            var result = _service.MakePayment(request);

            // Assert
            Assert.Equal(result.Success, expected);
        }
    }
}
