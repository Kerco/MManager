using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Template10.Mvvm;
using UI.Model;
using UI.Services;
using UI.Views;
using Windows.UI.Xaml.Controls;

namespace UI.ViewModels
{
    class RegisterPageViewModel : ViewModelBase
    {
        public ObservableCollection<String> GenderType { get; set; } = new ObservableCollection<string>();
        private string _gender;
        private DateTimeOffset _birthday;
        private string _name;
        private string _email;
        private string _psw;
        private string _confirmpsw;
        public DelegateCommand Register { get; }
        public DelegateCommand Cancel { get; }
        private bool _errorUserName;
        private bool _errorEmail;
        private bool _errorPsw;
        private bool _errorPswConfirm;
        private bool _hasError;

        public bool HasError
        {
            get { return !_hasError; }
            set
            {
                Set(ref _hasError, value);
            }
        }

        public bool ErrorUserName
        {
            get { return !_errorUserName; }
            set
            {
                Set(ref _errorUserName, value);
            }
        }

        public bool ErrorEmail
        {
            get { return !_errorEmail; }
            set
            {
                Set(ref _errorEmail, value);
            }
        }

        public bool ErrorPsw
        {
            get { return !_errorPsw; }
            set
            {
                Set(ref _errorPsw, value);
            }
        }

        public bool ErrorPswConfirm
        {
            get { return !_errorPswConfirm; }
            set
            {
                Set(ref _errorPswConfirm, value);
            }
        }
        public string ConfirmPassword
        {
            get { return _confirmpsw; }
            set
            {
                Set(ref _confirmpsw, value);
                CheckPasswordEquals();
            }
        }
        public string Password
        {
            get { return _psw; }
            set
            {
                Set(ref _psw, value);
                CheckPassword();
            }
        }
        public string EmailAddress
        {
            get { return _email; }
            set
            {
                Set(ref _email, value);
                CheckEmail();
            }
        }
        public string UserName
        {
            get { return _name; }
            set
            {
                Set(ref _name, value);
                CheckUserName();
            }
        }
        public DateTimeOffset BirthDay
        {
            get { return _birthday; }
            set
            {
                Set(ref _birthday, value);
                CheckError();
            }
        }
        public string SelectedGender
        {
            get { return _gender; }
            set
            {
                Set(ref _gender, value);
                CheckError();
            }
        }
        public RegisterPageViewModel()
        {
            Register = new DelegateCommand(Registration);
            Cancel = new DelegateCommand(NavigatoBack);
            BirthDay = System.DateTime.Now;
            GenderType.Add("Male");
            GenderType.Add("Female");
            ErrorEmail = true;
            ErrorPsw = true;
            ErrorPswConfirm = true;
            ErrorUserName = true;
            HasError = true;
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

            var res = await service.RegsiterAsync(model);
            if (res == null)
            {
                ContentDialog LoginFailedDialog = new ContentDialog
                {
                    Title = "Registration Failed",
                    Content = "Please check your input parameters and try again",
                    CloseButtonText = "Try Again"
                };

                ContentDialogResult result = await LoginFailedDialog.ShowAsync();
            }
            else
            {
                var acc = await service.GetAccountByEmail(EmailAddress);
                NavigationService.Navigate(typeof(EnvelopePage), acc.Id);
            }
        }
        private void NavigatoBack()
        {
            NavigationService.Navigate(typeof(LoginPage));
        }

        /// <summary>
        /// Checks if the UserName is eligible.
        /// </summary>
        public void CheckUserName()
        {
            if (String.IsNullOrWhiteSpace(UserName))
                ErrorUserName = true;
            else
                ErrorUserName = false;
            CheckError();
        }
        /// <summary>
        /// Checks if the Email is eligible.
        /// </summary>
        public void CheckEmail()
        {
            string pattern = null;
            pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

            if (Regex.IsMatch(EmailAddress, pattern))
                ErrorEmail = false;
            else
                ErrorEmail = true;
            CheckError();
        }
        /// <summary>
        /// Checks if the Password is eligible.
        /// </summary>
        public void CheckPassword()
        {
            if (Password.Length < 8)
                ErrorPsw = true;
            else
                ErrorPsw = false;
            CheckError();
        }
        /// <summary>
        /// Checks if the two password is equals.
        /// </summary>
        private void CheckPasswordEquals()
        {
            if (string.Equals(Password, ConfirmPassword))
                ErrorPswConfirm = false;
            else
                ErrorPswConfirm = true;
            CheckError();
        }
        /// <summary>
        /// Checks whether everything is eligible.
        /// </summary>
        private void CheckError()
        {
            if (ErrorUserName == false)
            {
                HasError = true;
                return;
            }
            else HasError = false;
            if (ErrorEmail == false)
            {
                HasError = true;
                return;
            }
            else HasError = false;
            if (ErrorPsw == false)
            {
                HasError = true;
                return;
            }
            else HasError = false;
            if (ErrorPswConfirm == false)
            {
                HasError = true;
                return;
            }
            else HasError = false;

            if (SelectedGender == null)
            {
                HasError = true;
                return;
            }
            else HasError = false;
        }
    }
}
