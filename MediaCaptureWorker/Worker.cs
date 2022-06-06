using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;

namespace MediaCaptureWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly MediaCapture mediaCapture_image;
        private readonly List<InMemoryRandomAccessStream> captureStreamList;
        private InMemoryRandomAccessStream captureStream;

        public Worker(ILogger<Worker> logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
            mediaCapture_image = new MediaCapture();
            captureStreamList = new List<InMemoryRandomAccessStream>();
        }

        public async override Task StartAsync(CancellationToken cancellationToken)
        {
            await mediaCapture_image.InitializeAsync();
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        captureStream = new InMemoryRandomAccessStream();
                        await mediaCapture_image.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg(), captureStream);
                        _logger.LogInformation("Image captured at: {time}", DateTimeOffset.Now);
                        captureStreamList.Add(captureStream);
                        await Task.Delay(1000, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Global exception occurred. Will resume in a moment.");
                    }
                }
            }
            finally
            {
                _logger.LogCritical("Exiting application...");
                _hostApplicationLifetime.StopApplication();
            }
        }

        public async override Task StopAsync(CancellationToken cancellationToken)
        {
            Save(captureStreamList);
            mediaCapture_image.Dispose();
            await base.StopAsync(cancellationToken);
        }

        private static async void Save(List<InMemoryRandomAccessStream> captureStreamList)
        {

            StorageFolder localFolder = await StorageFolder.GetFolderFromPathAsync("D:\\FYP\\ExamIn\\ExamInDesktopUI.Library\\images");

            foreach (var stream in captureStreamList)
            {
                string fileName = "frame_" + DateTime.Now.ToString("mmssfff") + ".jpg";
                StorageFile file = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                using var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite);
                var decoder = await BitmapDecoder.CreateAsync(stream);
                var encoder = await BitmapEncoder.CreateForTranscodingAsync(fileStream, decoder);
                var properties = new BitmapPropertySet
                {
                    { "System.Photo.Orientation", new BitmapTypedValue(PhotoOrientation.Normal, PropertyType.UInt16) }
                };
                await encoder.BitmapProperties.SetPropertiesAsync(properties);
                await encoder.FlushAsync();
            }
        }
    }
}
