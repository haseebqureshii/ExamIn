using Caliburn.Micro;
using ExamInDesktopUI.EventModels;
using System.Threading;
using System.Threading.Tasks;

namespace ExamInDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>, IHandle<ExamEvent>
    {
        private IEventAggregator _events;
        private DashboardViewModel _dashVM;
        private ExamViewModel _examVM;

        [System.Obsolete]
        public ShellViewModel(IEventAggregator events, DashboardViewModel DashVM, ExamViewModel examVM)
        {
            _events = events;
            _dashVM = DashVM;
            _examVM = examVM;

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
    }
}
