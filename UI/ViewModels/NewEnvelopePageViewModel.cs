using System;
using System.Collections.Generic;
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
    class NewEnvelopePageViewModel : Template10.Mvvm.ViewModelBase
    {
        private string _envelopeName;
        private int _envelopeValue;
        private string _envelopeDetails;
        private Envelope _envelope;
        private bool _hasError;
        string accID;
        private bool _errorName;
        private bool _errorValue;
        public DelegateCommand CreateNewTransactionCommand { get; }
        public DelegateCommand CancelCommand { get; }
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
        public Envelope Envelope
        {
            get { return _envelope; }
            set
            {
                Set(ref _envelope, value);
            }
        }
        public string EnvelopeDetails
        {
            get { return _envelopeDetails; }
            set
            {
                Set(ref _envelopeDetails, value);
                CheckError();
            }
        }
        public int EnvelopeValue
        {
            get { return _envelopeValue; }
            set
            {
                Set(ref _envelopeValue, value);
                CheckValue();
            }
        }
        public string EnvelopeName
        {
            get { return _envelopeName; }
            set
            {
                Set(ref _envelopeName, value);
                CheckName();
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
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            accID = (string)parameter;
            await base.OnNavigatedToAsync(parameter, mode, state);
            ErrorName = true;
            ErrorValue = true;
            HasError = true;
        }
        public NewEnvelopePageViewModel()
        {
            CreateNewTransactionCommand = new DelegateCommand(CreateAsync);
            CancelCommand = new DelegateCommand(Cancel);
        }
        private void Cancel()
        {
            NavigationService.Navigate(typeof(EnvelopePage));
        }
        private async void CreateAsync()
        {
            var service = new EnvelopeManager();

            Envelope el = new Envelope
            {
                Name = EnvelopeName,
                Value = EnvelopeValue,
                AccountId = accID,
                Details = EnvelopeDetails
            };
            await service.CreateEnvelopeAsync(el);
            NavigationService.Navigate(typeof(EnvelopePage), accID);

        }
        private void CheckName()
        {
            if (string.IsNullOrWhiteSpace(EnvelopeName))
                ErrorName = true;
            else
                ErrorName = false;
            CheckError();
        }

        private void CheckValue()
        {
            if (EnvelopeValue <= 0)
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
        }
    }
}
