
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
    class LoginPageViewModel : ViewModelBase
    {
        private string _email;
        private string _psw;
        public DelegateCommand Login { get; }
        public DelegateCommand Register { get; }

        public string Password
        {
            get { return _psw; }
            set
            {
                Set(ref _psw, value);
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

        public LoginPageViewModel()
        {
            Login = new DelegateCommand(LoginAsync);
            Register = new DelegateCommand(NavigateToRegister);
        }

        private void NavigateToRegister()
        {
            NavigationService.Navigate(typeof(RegisterPage));
        }

        private async void LoginAsync()
        {
            var service = new AccountManager();

            var acc = await service.GetAccountByUserName(EmailAddress);
            LoginViewModel model = new LoginViewModel
            {
                Email = EmailAddress,
                Password = this.Password
            };

            await service.LogInAsync(model);

            //var acc = await service.GetAccountByEmail(EmailAddress);

            NavigationService.Navigate(typeof(EnvelopePage), acc.Id);
        }

        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var service = new AccountManager();
            await service.ClearCookiesAsnyc();
            await base.OnNavigatedToAsync(parameter, mode, state);
        }
    }
}
