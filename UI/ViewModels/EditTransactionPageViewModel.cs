using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using UI.Model;
using UI.Services;
using UI.Views;
using Windows.UI.Xaml.Navigation;

namespace UI.ViewModels
{
    class EditTransactionPageViewModel : ViewModelBase
    {
        public ObservableCollection<String> TransactionTypes { get; set; } = new ObservableCollection<String>();
        private Transaction _transaction;
        public static int transactionID;
        private string _tranName;
        private DateTimeOffset _tranDate;
        private int _tranValue;
        private bool _hasError = true;
        private string _tranType;
        private string _tranDetails;
        public static int envelopeID;
        private bool _errorName;
        private bool _errorValue;
        public bool ErrorName
        {
            get { return !_errorName; }
            set
            {
                Set(ref _errorName, value);
            }
        }
        public bool ErrorValue
        {
            get { return !_errorValue; }
            set
            {
                Set(ref _errorValue, value);
            }
        }
        public Transaction Transaction
        {
            get { return _transaction; }
            set
            {
                Set(ref _transaction, value);
                CheckError();
            }
        }
        public string TranDetails
        {
            get { return _tranDetails; }
            set
            {
                Set(ref _tranDetails, value);
                CheckError();
            }
        }
        public string TranType
        {
            get { return _tranType; }
            set
            {
                Set(ref _tranType, value);
                CheckError();
            }
        }
        public DateTimeOffset TranDate
        {
            get { return _tranDate; }
            set
            {
                Set(ref _tranDate, value);
                CheckError();

            }
        }
        public bool HasError
        {
            get { return !_hasError; }
            set
            {
                Set(ref _hasError, value);
            }
        }
        public string TranName
        {
            get { return _tranName; }
            set
            {
                Set(ref _tranName, value);
                CheckName();
            }
        }
        public int TranValue
        {
            get { return _tranValue; }
            set
            {
                Set(ref _tranValue, value);
                CheckValue();
            }
        }
        public DelegateCommand EditTransactionCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public EditTransactionPageViewModel()
        {
            EditTransactionCommand = new DelegateCommand(EditAsync);
            CancelCommand = new DelegateCommand(Cancel);
            TransactionTypes.Add("Income");
            TransactionTypes.Add("Expense");
        }

        private async void EditAsync()
        {
            var service = new TransactionManager();
            Transaction tran = new Transaction
            {
                Id = transactionID,
                Name = TranName,
                Value = TranValue,
                EnvelopeId = envelopeID,
                Date = TranDate.DateTime,
                Details = TranDetails
            };

            await service.EditTransactionAsync(tran);

            NavigationService.Navigate(typeof(EnvelopeDetails), envelopeID);

        }
        private void Cancel()
        {
            NavigationService.Navigate(typeof(EnvelopeDetails), envelopeID);
        }
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            transactionID = (int)parameter;
            var service = new TransactionManager();

            Transaction = await service.EditAsync(transactionID);
            TranName = Transaction.Name;
            TranValue = Transaction.Value;
            TranDetails = Transaction.Details;
            envelopeID = Transaction.EnvelopeId;
            TranDate = Transaction.Date;

            if (Transaction.Type == true)
            {
                TranType = TransactionTypes[0];
            }
            else if (Transaction.Type == false)
            {
                TranType = TransactionTypes[1];
            }

        }
        /// <summary>
        /// Checks if the Name is eligible.
        /// </summary>
        public void CheckName()
        {
            if (string.IsNullOrWhiteSpace(TranName))
            {
                ErrorName = true;

            }
            else
                ErrorName = false;

            CheckError();
        }
        /// <summary>
        /// Checks if the Value is eligible.
        /// </summary>
        public void CheckValue()
        {
            if (TranValue <= 0)
            {
                ErrorValue = true;
            }
            else ErrorValue = false;

            CheckError();
        }
        /// <summary>
        /// Checks if everything is eligible.
        /// </summary>
        public void CheckError()
        {
            if (ErrorName == false)
            {
                HasError = true;
                return;
            }
            else HasError = false;

            if (ErrorValue == false)
            {
                HasError = true;
                return;
            }
            else HasError = false;
        }
    }
}
