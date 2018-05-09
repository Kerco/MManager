using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using UI.Model;
using UI.Services;
using UI.Views;
using Windows.UI.Xaml.Navigation;

namespace UI.ViewModels
{
    class NewTransactionPageViewModel : ViewModelBase
    {
        public static int envelopeID;
        private string _tranName;
        private DateTimeOffset _tranDate;
        private int _tranValue;
        private bool _hasError = true;
        private string _tranType;
        private string _tranDetails;
        private bool _errorName;
        private bool _errorValue;
  
        public DelegateCommand CreateTransactionCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public ObservableCollection<String> TransactionTypes { get; set; } = new ObservableCollection<String>();

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
        public NewTransactionPageViewModel()
        {
            CreateTransactionCommand = new DelegateCommand(CreateAsync);
            CancelCommand = new DelegateCommand(Cancel);
            TranDate = System.DateTime.Now;
            TransactionTypes.Add("Income");
            TransactionTypes.Add("Expense");
        }

        private void Cancel()
        {
            NavigationService.Navigate(typeof(EnvelopeDetails), envelopeID);
        }

        private async void CreateAsync()
        {
            var service = new TransactionManager();

            Transaction tran = new Transaction
            {
                Name = TranName,
                Value = TranValue,
                EnvelopeId = envelopeID,
                Date = TranDate.DateTime,
                Details = TranDetails
            };
            if (String.Equals(TranType, TransactionTypes[0]))
                tran.Type = true;
            else if (String.Equals(TranType, TransactionTypes[1]))
                tran.Type = false;

            await service.CreateTransactionAsync(tran);

            NavigationService.Navigate(typeof(EnvelopeDetails), envelopeID);
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            envelopeID = (int)parameter;
            ErrorName = true;
            ErrorValue = true;
            HasError = true;
            await base.OnNavigatedToAsync(parameter, mode, state);
        }

        private void CheckName()
        {
            if (string.IsNullOrWhiteSpace(TranName))
                ErrorName = true;
            else
                ErrorName = false;
            CheckError();
        }
        private void CheckValue()
        {
            if (TranValue <= 0)
                ErrorValue = true;

            else ErrorValue = false;
            CheckError();
        }
        private void CheckError()
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

            if (TranType == null)
            {
                HasError = true;
                return;
            }
            else HasError = false;
        }
    }
}
