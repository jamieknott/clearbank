using Castle.Core.Logging;
using ClearBank.DeveloperTest.Factories;
using ClearBank.DeveloperTest.Interfaces;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        private readonly Mock<IDataService> _mockDataService;

        private readonly PaymentService _service;

        public PaymentServiceTests()
        {
            _mockDataService = new Mock<IDataService>();

            var _paymentSchemeFactory = new PaymentSchemeFactory();
            _service = new PaymentService(
                _mockDataService.Object,
                _paymentSchemeFactory,
                new Mock<ILogger<PaymentService>>().Object
            );
        }

        [Fact]
        public void MakePayment_ReturnsFailure()
        {
            // Act
            _mockDataService.Setup(x => x.GetAccount(It.IsAny<string>())).Returns((Account)null);

            // Arrange
            var result = _service.MakePayment(new MakePaymentRequest());

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public void MakePayment_ReturnsSuccess()
        {
            // Arrange
            var account = new Account { AccountNumber = "123", Balance = 100, AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs };
            _mockDataService.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            var request = new MakePaymentRequest { DebtorAccountNumber = "123", PaymentScheme = PaymentScheme.Bacs, Amount = 50 };

            // Act
            var result = _service.MakePayment(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(50, account.Balance);
            _mockDataService.Verify(x => x.UpdateAccount(account), Times.Once);
        }

        [Theory]
        [InlineData(PaymentScheme.Bacs, true)]
        [InlineData(PaymentScheme.Chaps, false)]
        public void MakePayment_BacsScheme_Validation_ReturnsExpected(PaymentScheme requestPaymentScheme, bool expected)
        {
            // Act
            var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs };
            _mockDataService.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

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
            _mockDataService.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

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
            _mockDataService.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            var request = new MakePaymentRequest { DebtorAccountNumber = "123", PaymentScheme = requestPaymentScheme, Amount = requestAmount };

            // Arrange
            var result = _service.MakePayment(request);

            // Assert
            Assert.Equal(result.Success, expected);
        }
    }
}
