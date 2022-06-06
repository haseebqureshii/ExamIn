using Caliburn.Micro;
using ExamInDesktopUI.EventModels;
using ExamInDesktopUI.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ExamInDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>, IHandle<ExamEvent>, IHandle<SignUpEvent>
    {
        private IEventAggregator _events;
        private IAPIHelper _apiHelper;
        private DashboardViewModel _dashVM;
        private ExamViewModel _examVM;
        private SignUpViewModel _signUpVM;

        [System.Obsolete]
        public ShellViewModel(IEventAggregator events, IAPIHelper apiHelper, DashboardViewModel DashVM, ExamViewModel examVM, SignUpViewModel signUpVM)
        {
            _events = events;
            _apiHelper = apiHelper;
            _dashVM = DashVM;
            _examVM = examVM;
            _signUpVM = signUpVM;

            _events.Subscribe(this);
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_dashVM);
        }

        public async Task HandleAsync(ExamEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_examVM);
        }

        public async Task HandleAsync(SignUpEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_signUpVM);
        }

        public async Task Logout()
        {
            try
            {
                await _apiHelper.Logout();
                await ActivateItemAsync(IoC.Get<LoginViewModel>());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
