using ClearBank.DeveloperTest.Interfaces;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IDataService _dataService;
        private readonly IPaymentSchemeFactory _paymentSchemeFactory;

        public PaymentService(IDataService dataService, IPaymentSchemeFactory paymentSchemeFactory)
        {
            _dataService = dataService;
            _paymentSchemeFactory = paymentSchemeFactory;
        }
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var result = new MakePaymentResult { Success = false };
            var account = _dataService.GetAccount(request.DebtorAccountNumber);
            if (account == null)
            {
                return result;
            }

            var scheme = _paymentSchemeFactory.GetScheme(request.PaymentScheme);

            if (scheme.IsValid(account, request))
            {
                account.Balance -= request.Amount;
                _dataService.UpdateAccount(account);
                result.Success = true;
            }

            return result;
        }
    }
}
