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

namespace UI.ViewModels
{
    class RegisterPageViewModel : ViewModelBase
    {
        private string _gender;
        private DateTimeOffset _birthday;
        private string _name;
        private string _email;
        private string _psw;
        private string _confirmpsw;
        public DelegateCommand Register { get; }
        public DelegateCommand Cancel { get; }

        public string ConfirmPassword
        {
            get { return _confirmpsw; }
            set
            {
                Set(ref _confirmpsw, value);
            }
        }


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


        public string UserName
        {
            get { return _name; }
            set
            {
                Set(ref _name, value);
            }
        }


        public DateTimeOffset BirthDay
        {
            get { return _birthday; }
            set
            {
                Set(ref _birthday, value);
            }
        }


        public string SelectedGender
        {
            get { return _gender; }
            set
            {
                Set(ref _gender, value);
            }
        }

        public ObservableCollection<String> GenderType { get; set; } = new ObservableCollection<string>();


        public RegisterPageViewModel()
        {
            Register = new DelegateCommand(Registration);
            Cancel = new DelegateCommand(NavigatoBack);
            BirthDay = System.DateTime.Now;
            GenderType.Add("Male");
            GenderType.Add("Female");
        }

        private async void Registration()
        {
            var service = new AccountManager();

            RegisterModel model = new RegisterModel
            {
                UserName = this.UserName,
                Password = this.Password,
                ConfirmPassword = this.ConfirmPassword,
                Email = this.EmailAddress,
                BirthDay = this.BirthDay.DateTime
            };
            if (String.Equals(SelectedGender, GenderType[0]))
                model.Gender = true;
            else if (String.Equals(SelectedGender, GenderType[1]))
                model.Gender = false;

            await service.RegsiterAsync(model);

            var acc = await service.GetAccountByEmail(EmailAddress);

            NavigationService.Navigate(typeof(EnvelopePage), acc.Id);
        }

        private void NavigatoBack()
        {
            NavigationService.Navigate(typeof(LoginPage));
        }
    }
}
