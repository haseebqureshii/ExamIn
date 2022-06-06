using Caliburn.Micro;
using ExamInDesktopUI.EventModels;
using ExamInDesktopUI.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace ExamInDesktopUI.ViewModels
{
    public class SignUpViewModel : Screen
    {
        private string _firstName;
        private string _lastName;
        private string _userName;
        private string _password;
        private string _confirmPassword;
        private string _errorMessage;
        private bool _flag;
        private IAPIHelper _apiHelper;
        private IEventAggregator _events;

        public SignUpViewModel(IAPIHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
            Flag = true;
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                NotifyOfPropertyChange(() => FirstName);
                NotifyOfPropertyChange(() => CanRegisterUser);
                NotifyOfPropertyChange(() => CanCaptureFace);
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                NotifyOfPropertyChange(() => LastName);
                NotifyOfPropertyChange(() => CanRegisterUser);
                NotifyOfPropertyChange(() => CanCaptureFace);
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanRegisterUser);
                NotifyOfPropertyChange(() => CanCaptureFace);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanRegisterUser);
                NotifyOfPropertyChange(() => CanCaptureFace);
            }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                NotifyOfPropertyChange(() => ConfirmPassword);
                NotifyOfPropertyChange(() => CanRegisterUser);
                NotifyOfPropertyChange(() => CanCaptureFace);
            }
        }

        public bool IsErrVisible
        {
            get
            {
                bool output = false;

                if (ErrorMessage?.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrVisible);
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }

        public bool Flag
        {
            get { return _flag; }
            set
            {
                _flag = value;
                NotifyOfPropertyChange(() => CanRegisterUser);
                NotifyOfPropertyChange(() => CanCaptureFace);
            }
        }

        public bool CanRegisterUser
        {
            get
            {
                bool output = false;
                if (UserName?.Length > 0 && 
                    Password?.Length > 0 &&
                    FirstName?.Length > 0 &&
                    LastName?.Length > 0 &&
                    ConfirmPassword?.Length > 0  &&
                    Flag == false &&
                    Password == ConfirmPassword)
                {
                    output = true;
                }
                return output;
            }
        }

        public bool CanCaptureFace
        {
            get
            {
                bool output = false;
                if (UserName?.Length > 0 &&
                    Password?.Length > 0 &&
                    FirstName?.Length > 0 &&
                    LastName?.Length > 0 &&
                    Flag == true)
                {
                    output = true;
                }
                return output;
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }

        public async Task CaptureFace()
        {
            try
            {
                IsLoading = true;
                if (await _apiHelper.SaveNewUserFace(UserName))
                {
                    Flag = false;
                }
                IsLoading = false;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                ErrorMessage = ex.Message;
            }
        }

        public async Task RegisterUser()
        {
            try
            {
                IsLoading = true;

                string res = string.Empty;
                if (await _apiHelper.RegisterUser(UserName, Password))
                {
                    var result = await _apiHelper.Authenticate(UserName, Password);
                    await _apiHelper.SetLoggedInUserInfo(result.Access_Token, FirstName, LastName, UserName, Password);
                    await _apiHelper.GetLoggedInUserInfo(result.Access_Token);
                }
                else
                    res = "Problem Registering User. Check Account WebApi";
                MessageBox.Show("User Registered.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                await _events.PublishOnUIThreadAsync(new LogOnEvent());

                IsLoading = false;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                ErrorMessage = ex.Message;
            }
        }
    }
}
