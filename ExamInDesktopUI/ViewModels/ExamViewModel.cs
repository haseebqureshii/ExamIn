using Caliburn.Micro;
using ExamInDesktopUI.Library.Api;
using ExamInDesktopUI.Library.Models;
using System.ComponentModel;
using System.Threading.Tasks;
using System;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Media.MediaProperties;
using Windows.Graphics.Imaging;
using Windows.Storage.FileProperties;
using Windows.Foundation;
using System.Timers;
using System.Windows.Threading;
using ExamInDesktopUI.EventModels;

namespace ExamInDesktopUI.ViewModels
{
    public class ExamViewModel : Screen
    {
        private IGetExamApi _examEndPoint;
        private ICloudApi _cloudEndPoint;
        private IEventAggregator _events;
        LowLagMediaRecording _mediaRecording;
        MediaCapture mediaCapture_audio;
        DateTime start;
        private string _timerDisplay;
        Timer aTimer = new Timer();
        DispatcherTimer t;

        public ExamViewModel(IGetExamApi examEndPoint, ICloudApi cloudEndPoint, IEventAggregator events)
        {
            _examEndPoint = examEndPoint;
            _cloudEndPoint = cloudEndPoint;
            _events = events;

            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 3500;
        }

        private async void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            await TakeSnapshotAsync();
            _ = Task.Run(() => SendImage());
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            t = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Background,
                TimerTick, Dispatcher.CurrentDispatcher);

            //await TakeSnapshotAsync();
            aTimer.Start();
            _ = Task.Run(async() => await LoadQuestions());
            await StartAudio();

            t.IsEnabled = true;
            start = DateTime.Now;
        }

        private async Task StartAudio()
        {
            mediaCapture_audio = new MediaCapture();
            await mediaCapture_audio.InitializeAsync();

            StorageFolder localFolder = await StorageFolder.GetFolderFromPathAsync("D:\\FYP\\ExamIn\\ExamInDesktopUI.Library");
            StorageFile file = await localFolder.CreateFileAsync("audio.wav", CreationCollisionOption.ReplaceExisting);
            _mediaRecording = await mediaCapture_audio.PrepareLowLagRecordToStorageFileAsync(
                    MediaEncodingProfile.CreateWav(AudioEncodingQuality.Auto), file);
            await _mediaRecording.StartAsync();
        }

        private async Task StopAudio()
        {
            await _mediaRecording.StopAsync();
            await _mediaRecording.FinishAsync();
        }


        private void SendImage()
        {
            _cloudEndPoint.GetFace();
            //_cloudEndPoint.GetObject();
        }

        private async Task LoadQuestions()
        {
            var ques = await _examEndPoint.GetAll();
            QuestionList = new BindingList<ExamModel>(ques);
        }

        public async Task TakeSnapshotAsync()
        {
            var mediaCapture_Image = new MediaCapture();
            await mediaCapture_Image.InitializeAsync();

            var myPictures = await StorageFolder.GetFolderFromPathAsync("D:\\FYP\\ExamIn\\ExamInDesktopUI.Library\\images");
            StorageFile file = await myPictures.CreateFileAsync("frame.jpg", CreationCollisionOption.ReplaceExisting);

            using (var captureStream = new InMemoryRandomAccessStream())
            {
                var _imageEncodingProperties = ImageEncodingProperties.CreateJpeg();
                _imageEncodingProperties.Width = 100;
                _imageEncodingProperties.Height = 100;

                await mediaCapture_Image.CapturePhotoToStreamAsync(_imageEncodingProperties, captureStream);

                using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var decoder = await BitmapDecoder.CreateAsync(captureStream);
                    var encoder = await BitmapEncoder.CreateForTranscodingAsync(fileStream, decoder);

                    var properties = new BitmapPropertySet {
                        { "System.Photo.Orientation", new BitmapTypedValue(PhotoOrientation.Normal, PropertyType.UInt16) }
                    };
                    await encoder.BitmapProperties.SetPropertiesAsync(properties);
                    await encoder.FlushAsync();
                }
                mediaCapture_Image.Dispose();
            }
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
                _timerDisplay = value;
                NotifyOfPropertyChange(() => TimerDisplay);
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            TimerDisplay = Convert.ToString(DateTime.Now - start);
        }

        public async Task Submit()
        {
            aTimer.Stop();
            aTimer.Dispose();

            t.IsEnabled = false;

            await StopAudio();

            //_ = Task.Run(() => _cloudEndPoint.MonitorAudio());
            
            await _events.PublishOnUIThreadAsync(new LogOnEvent());
        }
    }
}
