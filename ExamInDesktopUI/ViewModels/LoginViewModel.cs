using Caliburn.Micro;
using ExamInDesktopUI.EventModels;
using ExamInDesktopUI.Helpers;
using System;
using System.Threading.Tasks;

namespace ExamInDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName;
        private string _password;
        private IAPIHelper _apiHelper;
        private IEventAggregator _events;

        public LoginViewModel(IAPIHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
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

        private string _errorMessage;
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

        public bool CanLogIn
        {
            get
            {
                bool output = false;
                if (UserName?.Length > 0 && Password?.Length > 0)
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

        public async Task SignUp()
        {
            await _events.PublishOnUIThreadAsync(new SignUpEvent());
        }

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = null;

                await LoadAuthenticate();

                await _events.PublishOnUIThreadAsync(new LogOnEvent());
            }
            catch (Exception ex)
            {
                IsLoading = false;
                ErrorMessage = ex.Message;
            }
        }

        private async Task LoadAuthenticate()
        {
            try
            {
                IsLoading = true;
                var result = await _apiHelper.Authenticate(UserName, Password);
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);
                if (await _apiHelper.AuthenticateUserFace() != true)
                {
                    throw new Exception("Face was not found.");
                }
                IsLoading = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
