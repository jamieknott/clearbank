using ClearBank.DeveloperTest.Interfaces;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Logging;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IDataService _dataService;
        private readonly IPaymentSchemeFactory _paymentSchemeFactory;
        private readonly ILogger _logger;

        public PaymentService(IDataService dataService, IPaymentSchemeFactory paymentSchemeFactory, ILogger logger)
        {
            _dataService = dataService;
            _paymentSchemeFactory = paymentSchemeFactory;
            _logger = logger;
        }
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var result = new MakePaymentResult { Success = false };
            var account = _dataService.GetAccount(request.DebtorAccountNumber);
            if (account == null)
            {
                _logger.LogWarning("Account {accountNumber} not found.", request.DebtorAccountNumber);
                return result;
            }

            var scheme = _paymentSchemeFactory.GetScheme(request.PaymentScheme);
            if (scheme == null)
            {
                _logger.LogError("No scheme found for payment scheme {scheme}.", request.PaymentScheme);
                return result;
            }

            if (scheme.IsValid(account, request))
            {
                account.Balance -= request.Amount;
                _dataService.UpdateAccount(account);
                result.Success = true;

                _logger.LogInformation("Payment of {amount} successful for account {accountNumber} using payment scheme {scheme}.",
                    request.Amount, account.AccountNumber, request.PaymentScheme);
                return result;
            }
            else
            {
                _logger.LogInformation("Validation failed for account {accountNumber} using payment scheme {scheme}.",
                    account.AccountNumber, request.PaymentScheme);
                return result;
            }
        }
    }
}
