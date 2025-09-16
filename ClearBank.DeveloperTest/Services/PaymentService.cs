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
            var account = _dataService.GetAccount(request.DebtorAccountNumber);

            //if (dataStoreType == "Backup")
            //{
            //    var accountDataStore = new BackupAccountDataStore();
            //    account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            //}
            //else
            //{
            //    var accountDataStore = new AccountDataStore();
            //    account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            //}

            var result = new MakePaymentResult();

            result.Success = true;
            
            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                    {
                        result.Success = false;
                    }
                    break;

                case PaymentScheme.FasterPayments:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                    {
                        result.Success = false;
                    }
                    else if (account.Balance < request.Amount)
                    {
                        result.Success = false;
                    }
                    break;

                case PaymentScheme.Chaps:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                    {
                        result.Success = false;
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

                //if (dataStoreType == "Backup")
                //{
                //    var accountDataStore = new BackupAccountDataStore();
                //    accountDataStore.UpdateAccount(account);
                //}
                //else
                //{
                //    var accountDataStore = new AccountDataStore();
                //    accountDataStore.UpdateAccount(account);
                //}

                _dataService.UpdateAccount(account);
            }

            return result;
        }
    }
}
