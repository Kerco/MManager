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

        public DelegateCommand CreateTransactionCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public ObservableCollection<String> TransactionTypes { get; set; } = new ObservableCollection<String>();

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
                CheckError();

            }
        }


        public int TranValue
        {
            get { return _tranValue; }
            set
            {
                Set(ref _tranValue, value);
                CheckError();
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

            await base.OnNavigatedToAsync(parameter, mode, state);
        }

        private void CheckError()
        {
            if (string.IsNullOrEmpty(TranName))
            {
                HasError = true;
                return;
            }
            else HasError = false;

            if (TranValue <= 0)
            {
                HasError = true;
                return;
            }
            else HasError = false;

            if (TranDate == null)
            {

                HasError = true;
                return;
            }
            else HasError = false;

            if (string.IsNullOrEmpty(TranType))
            {

                HasError = true;
                return;
            }
            else HasError = false;

        }


    }
}
