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
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UI.ViewModels
{
    class EnvelopePageViewModel : ViewModelBase
    {
        public ObservableCollection<Envelope> Envelopes { get; set; } = new ObservableCollection<Envelope>();
        public ObservableCollection<EnvelopePageViewModel> EnvelopesViewModel { get; set; } = new ObservableCollection<EnvelopePageViewModel>();
        private string _name;
        static string accID;
        private string _userName;
        private string _email;
        private string _gender;
        private string _birthDay;
        private bool _loading;
        private int _value;
        private Envelope _envelope;

        public string BirthDay
        {
            get { return _birthDay; }
            set
            {
                Set(ref _birthDay, value);
            }
        }

        public string Gender
        {
            get { return _gender; }
            set
            {
                Set(ref _gender, value);
            }
        }

        public string EmailAddress
        {
            get { return _email; }
            set
            {
                Set(ref _email, value);
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                Set(ref _userName, value);
            }
        }

        public bool Loading
        {
            get { return _loading; }
            set
            {
                Set(ref _loading, value);
            }
        }

        public string EnvelopeName
        {
            get { return _name; }
            set
            {
                Set(ref _name, value);
            }
        }

        public int EnvelopeValue
        {
            get { return _value; }
            set
            {
                Set(ref _value, value);
            }
        }
        public Envelope SelectedEnvelope
        {
            get { return _envelope; }
            set
            {
                Set(ref _envelope, value);
            }
        }

        public DelegateCommand Navigate { get; }
        public DelegateCommand CreateEnvelope { get; }
        public DelegateCommand Delete { get; }
        public DelegateCommand LogOut { get; }

        public EnvelopePageViewModel()
        {
            Navigate = new DelegateCommand(NavigateToDetails);
            CreateEnvelope = new DelegateCommand(NavigateToNewEnvelope);
            Delete = new DelegateCommand(DeleteAsync);
            LogOut = new DelegateCommand(LogOutAsync);
        }

        private async void LogOutAsync()
        {
            var service = new AccountManager();
            await service.LogOutAsync();

            NavigationService.Navigate(typeof(LoginPage));
        }

        private async void DeleteAsync()
        {
            ContentDialog Delete = new ContentDialog
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(1.5),
                Title = "Delete Envelope",
                Content = "Are you sure want to Delete " + SelectedEnvelope.Name + " Envelope?",
                CloseButtonText = "No",
                PrimaryButtonText = "Yes"
            };
            ContentDialogResult res = await Delete.ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                var service = new EnvelopeManager();
                bool result = await service.DeleteEnvelopeConfirmedAsync(SelectedEnvelope.Id);
                await LoadAsync();
            }
        }
        private void NavigateToNewEnvelope()
        {
            NavigationService.Navigate(typeof(NewEnvelopePage), accID);
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            accID = (string)parameter;
            Loading = true;
            await LoadAsync();
            await LoadAccount();
            Loading = false;

            await base.OnNavigatedToAsync(parameter, mode, state);
        }
        private async Task LoadAccount()
        {
            var service = new AccountManager();
            var account = await service.DetailsAsync(accID);

            UserName = account.UserName;
            EmailAddress = account.Email;
            if (account.Gender == true)
                Gender = "Male";
            else if (account.Gender == false)
                Gender = "Female";
            BirthDay = account.BirthDay.Year.ToString() + "." + account.BirthDay.Month.ToString() + "." + account.BirthDay.Day.ToString();

        }
        public void NavigateToDetails()
        {
            NavigationService.Navigate(typeof(EnvelopeDetails), SelectedEnvelope.Id);
        }
        private async Task LoadAsync()
        {
            Envelopes.Clear();
            var service = new EnvelopeManager();
            var envelopes = await service.GetEnvelopeByAccountAsync(accID);
            foreach (var item in envelopes)
            {
                Envelopes.Add(item);
            }
        }



    }
}
