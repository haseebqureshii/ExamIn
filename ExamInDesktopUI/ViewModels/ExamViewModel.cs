using Caliburn.Micro;
using ExamInDesktopUI.EventModels;
using ExamInDesktopUI.Library.Api;
using ExamInDesktopUI.Library.Models;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;

namespace ExamInDesktopUI.ViewModels
{
    public class ExamViewModel : Screen
    {
        private readonly IGetExamApi _examEndPoint;
        private readonly ICloudApi _cloudEndPoint;
        private readonly IEventAggregator _events;
        private readonly IMediaCaptureApi _mediaCapture;
        private ILoggedInUserModel _loggedInUser;
        private DateTime start;
        private string _timerDisplay = null;
        private readonly DispatcherTimer t;

        public ExamViewModel(IGetExamApi examEndPoint, ICloudApi cloudEndPoint, IEventAggregator events, IMediaCaptureApi mediaCaptureApi, ILoggedInUserModel loggedInUser)
        {
            _examEndPoint = examEndPoint;
            _cloudEndPoint = cloudEndPoint;
            _mediaCapture = mediaCaptureApi;
            _loggedInUser = loggedInUser;
            _events = events;

            t = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 1), DispatcherPriority.Background,
                TimerTick, Dispatcher.CurrentDispatcher);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await Task.Run(() => _mediaCapture.StartAll());
                await Task.Run(async () => await LoadQuestions());

                t.IsEnabled = true;
                start = DateTime.Now;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                await Task.Run(() => _mediaCapture.StopAll());
            }
        }

        private async Task LoadQuestions()
        {
            var ques = await _examEndPoint.GetAll();
            QuestionList = new BindingList<ExamModel>(ques);
        }

        private BindingList<ExamModel> _questionList;
        public BindingList<ExamModel> QuestionList
        {
            get { return _questionList; }
            set
            {
                _questionList = value;
                NotifyOfPropertyChange(() => QuestionList);
            }
        }

        public string TimerDisplay
        {
            get { return _timerDisplay; }
            set
            {
                _timerDisplay = "";
                _timerDisplay = value.Substring(0, 8);
                NotifyOfPropertyChange(() => TimerDisplay);
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            TimerDisplay = Convert.ToString(DateTime.Now - start);
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

        public HttpClient ApiClient { get; private set; }
        private async Task SendData()
        {
            string data = string.Empty;
            string url = @"https://result-added-admin-panel.herokuapp.com/questions/result/";
            var path = @"D:\FYP\ExamIn\ExamInDesktopUI.Library";
            foreach (string file in Directory.EnumerateFiles(path, "*.txt"))
            {
                    data += File.ReadAllText(file) + " ### ";
            }

            var reqData = new Dictionary<string, string>
            {
              {"report", $"{data}"},
              {"Name", $"{_loggedInUser.FirstName + " " + _loggedInUser.LastName}"},
              {"Email", $"{_loggedInUser.EmailAddress}"}
            };

            ApiClient = new HttpClient
            {
                BaseAddress = new Uri(url)
            };
            ApiClient.DefaultRequestHeaders.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (HttpResponseMessage response = await ApiClient.PostAsJsonAsync(url, reqData))    
            {
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Results submitted with success status.", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    ResendData(response.ReasonPhrase.ToString());
                }
            }
        }

        private async void ResendData(string err)
        {
            var result = MessageBox.Show(err + ". Do you wish to try again?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (result == MessageBoxResult.Yes)
            {
                await SendData();
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

        public async void Submit()
        {
            try
            {
                IsLoading = true;

                ErrorMessage = null;
                t.IsEnabled = false;
                _mediaCapture.StopAll();

                await StartModels();
                await SendData();
                _cloudEndPoint.DeleteImageFolder();

                IsLoading = false;
                await _events.PublishOnUIThreadAsync(new LogOnEvent());
            }
            catch (Exception ex)
            {
                IsLoading = false;
                ErrorMessage = ex.Message;
            }
        }

        private async Task StartModels()
        {
            try
            {
                //await _cloudEndPoint.GetFace();
                //await _cloudEndPoint.GetObject();
                //await _cloudEndPoint.MonitorAudio();
                await Task.WhenAll(Face(), Object(), Audio());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task Face()
        {
            try
            {
                await _cloudEndPoint.GetFace();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task Object()
        {
            try
            {
                await _cloudEndPoint.GetObject();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task Audio()
        {
            try
            {
                await _cloudEndPoint.MonitorAudio();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
