using ClearBank.DeveloperTest.Interfaces;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        private readonly PaymentService _service;
        private readonly Mock<IDataService> _mockDataService;

        public PaymentServiceTests()
        {
            _mockDataService = new Mock<IDataService>();

            _service = new PaymentService(_mockDataService.Object);
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
