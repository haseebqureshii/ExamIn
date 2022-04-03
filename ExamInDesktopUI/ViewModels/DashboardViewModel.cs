using Caliburn.Micro;
using ExamInDesktopUI.EventModels;
using ExamInDesktopUI.Library.Models;
using System;
using System.Threading.Tasks;

namespace ExamInDesktopUI.ViewModels
{
    public class DashboardViewModel : Screen
    {
        private ILoggedInUserModel _loggedInUser;
        private IEventAggregator _events;

        public DashboardViewModel(ILoggedInUserModel loggedInUser, IEventAggregator events)
        {
            _loggedInUser = loggedInUser;
            _events = events;
        }

        protected override void OnViewLoaded(object view)
        {
            FetchUserInfo();
            base.OnViewLoaded(view);
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
            }
        }

        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set 
            { 
                _fullName = value;
                NotifyOfPropertyChange(() => FullName);
            }
        }

        public void FetchUserInfo()
        {
            UserName =  _loggedInUser.EmailAddress;
            FullName = _loggedInUser.FirstName + " " + _loggedInUser.LastName;
        }

        private string _examLink;
        public string ExamLink
        {
            get { return _examLink; }
            set 
            { 
                _examLink = value;
                NotifyOfPropertyChange(() => ExamLink);
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

        public async Task NextAsync()
        {
            try
            {
                ErrorMessage = null;
                await _events.PublishOnUIThreadAsync(new ExamEvent());
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
