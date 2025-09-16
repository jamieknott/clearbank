using ClearBank.DeveloperTest.Interfaces;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IDataService _dataService;

        public PaymentService(IDataService dataService)
        {
            _dataService = dataService;
        }
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var result = new MakePaymentResult { Success = false };
            var account = _dataService.GetAccount(request.DebtorAccountNumber);
            if (account == null)
            {
                return result;
            }
            
            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    if (account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                    {
                        result.Success = true;
                    }
                    break;

                case PaymentScheme.FasterPayments:
                    if (account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                    {
                        result.Success = true;
                    }
                    else if (account.Balance < request.Amount)
                    {
                        result.Success = false;
                    }
                    break;

                case PaymentScheme.Chaps:
                    if (account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                    {
                        result.Success = true;
                    }
                    else if (account.Status != AccountStatus.Live)
                    {
                        result.Success = false;
                    }
                    break;
            }

            if (result.Success)
            {
                account.Balance -= request.Amount;

                _dataService.UpdateAccount(account);
            }

            return result;
        }
    }
}
