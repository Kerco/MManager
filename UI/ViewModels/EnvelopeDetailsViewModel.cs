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
    class EnvelopeDetailsViewModel : ViewModelBase
    {
        public ObservableCollection<Transaction> Transactions { get; set; } = new ObservableCollection<Transaction>();
        public static int envelopeID;
        public int remaining = 0;
        private string _envelopeName;
        private string _envelopeDetails;
        private Envelope _envelope;
        private int _income;
        private int _expense;
        private string _remainingMoney;
        private Transaction _selectedTransaction;
        private string _accUserName;
        private string _accEmail;
        private string _accGender;
        private string _accBirthDay;
        static string accID;

        public string AccBirthDay
        {
            get { return _accBirthDay; }
            set
            {
                Set(ref _accBirthDay, value);
            }
        }


        public string AccGender
        {
            get { return _accGender; }
            set
            {
                Set(ref _accGender, value);
            }
        }


        public string AccEmail
        {
            get { return _accEmail; }
            set
            {
                Set(ref _accEmail, value);
            }
        }


        public string AccUserName
        {
            get { return _accUserName; }
            set
            {
                Set(ref _accUserName, value);
            }
        }


        public Transaction SelecetedTransaction
        {
            get { return _selectedTransaction; }
            set
            {
                Set(ref _selectedTransaction, value);
            }
        }


        public string RemainingMoney
        {
            get { return _remainingMoney; }
            set
            {
                Set(ref _remainingMoney, value);
            }
        }


        public int Expense
        {
            get { return _expense; }
            set
            {
                Set(ref _expense, value);
            }
        }


        public int Income
        {
            get { return _income; }
            set
            {
                Set(ref _income, value);
            }
        }




        public DelegateCommand CreateNewTransactionCommand { get; }

        public DelegateCommand EditEnvelopeCommand { get; }
        public DelegateCommand EditTransactionCommand { get; }

        public DelegateCommand DeleteEnvelopeCommand { get; }

        public DelegateCommand DeleteTransactionCommand { get; }

        public DelegateCommand Logout { get; }

        public string EnvelopeDetails
        {
            get { return _envelopeDetails; }
            set
            {
                Set(ref _envelopeDetails, value);
            }
        }

        public string EnvelopeName
        {
            get { return _envelopeName; }
            set
            {
                Set(ref _envelopeName, value);
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

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            envelopeID = (int)parameter;
            await Load();
            await LoadAccount();

            await base.OnNavigatedToAsync(parameter, mode, state);
        }

        public EnvelopeDetailsViewModel()
        {
            CreateNewTransactionCommand = new DelegateCommand(NavigateToNewTransaction);
            EditEnvelopeCommand = new DelegateCommand(NavigateToEditEnvelope);
            EditTransactionCommand = new DelegateCommand(NavigateToEditTransaction);
            DeleteEnvelopeCommand = new DelegateCommand(DeleteConfirmation);
            DeleteTransactionCommand = new DelegateCommand(DeleteTransactionAsync);
            Logout = new DelegateCommand(LogOutAsync);
        }

        private async void DeleteTransactionAsync()
        {
            var contentDialog = new DeleteTransaction(SelecetedTransaction.Id);
            await contentDialog.ShowAsync();

            await Load();

        }

        private async void DeleteConfirmation()
        {
            var contentDialog = new DeleteConfirmation(envelopeID);
            await contentDialog.ShowAsync();
        }

        private void NavigateToEditTransaction()
        {
            NavigationService.Navigate(typeof(EditTransactionPage), SelecetedTransaction.Id);
        }

        private void NavigateToEditEnvelope()
        {
            NavigationService.Navigate(typeof(EditEnvelopePage), envelopeID);
        }

        public void NavigateToNewTransaction()
        {
            NavigationService.Navigate(typeof(NewTransactionPage), envelopeID);
        }

        private async void LogOutAsync()
        {
            var service = new AccountManager();
            await service.LogOutAsync();

            NavigationService.Navigate(typeof(LoginPage));
        }

        private async Task Load()
        {
            Transactions.Clear();
            var service = new EnvelopeManager();
            Envelope = await service.GetEnvelopeDetailsWithTransactionAsync(envelopeID);
            Income = 0;
            Expense = 0;


            foreach (var item in Envelope.Transactions)
            {
                if (item.Type == false)
                    Expense += item.Value;
                else if (item.Type == true)
                    Income += item.Value;
                Transactions.Add(item);
            }
            EnvelopeName = Envelope.Name;
            EnvelopeDetails = Envelope.Details;
            Income += Envelope.Value;
            remaining = Income - Expense;
            RemainingMoney = remaining.ToString();
            accID = Envelope.AccountId;

        }

        private async Task LoadAccount()
        {
            var service = new AccountManager();
            var account = await service.DetailsAsync(accID);

            AccUserName = account.UserName;
            AccEmail = account.Email;
            if (account.Gender == true)
                AccGender = "Male";
            else if (account.Gender == false)
                AccGender = "Female";
            AccBirthDay = account.BirthDay.Year.ToString() + "." + account.BirthDay.Month.ToString() + "." + account.BirthDay.Day.ToString();

        }


    }
}
