using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;

namespace ExamInDesktopUI.Library.Api
{
    public class MediaCaptureApi : IMediaCaptureApi
    {
        private ServiceController service;
        private MediaCapture mediaCapture_audio;
        private LowLagMediaRecording _audioRecording;

        public MediaCaptureApi()
        {
            mediaCapture_audio = new MediaCapture();
        }

        public void StartAll()
        {
            StartImageCapture();
            StartAudioCapture();
        }

        public void StopAll()
        {
            StopImageCapture();
            StopAudioCapture();
        }

        public void StartImageCapture()
        {
            service = new ServiceController("ExamInMediaCaptureService");  
            if ((service.Status.Equals(ServiceControllerStatus.Stopped)) ||
                    (service.Status.Equals(ServiceControllerStatus.StopPending)))
                service.Start();

            else service.Stop();
        }

        public void StopImageCapture()
        {
            service.Stop();
            service.Dispose();
        }

        public async void StartAudioCapture()
        {
            await mediaCapture_audio.InitializeAsync();

            StorageFolder localFolder = await StorageFolder.GetFolderFromPathAsync(@"D:\FYP\ExamIn\ExamInDesktopUI.Library");
            StorageFile file = await localFolder.CreateFileAsync("audio.wav", CreationCollisionOption.ReplaceExisting);
            _audioRecording = await mediaCapture_audio.PrepareLowLagRecordToStorageFileAsync(
                    MediaEncodingProfile.CreateWav(AudioEncodingQuality.High), file);

            await _audioRecording.StartAsync();
        }

        public async void StopAudioCapture()
        {
            await _audioRecording.FinishAsync();
            mediaCapture_audio.Dispose();
        }

        public async Task<InMemoryRandomAccessStream> CaptureSingleImage()
        {
            var captureStream = new InMemoryRandomAccessStream();
            var mediaCapture_image = new MediaCapture();
            await mediaCapture_image.InitializeAsync();
            await mediaCapture_image.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg(), captureStream);
            mediaCapture_image.Dispose();
            return captureStream;
        }
    }
}
