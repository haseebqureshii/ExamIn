using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace ExamInDesktopUI.Library.Api
{
    public interface IMediaCaptureApi
    {
        void StartAll();
        void StopAll();
        void StartImageCapture();
        void StopImageCapture();
        void StartAudioCapture();
        void StopAudioCapture();
        Task<InMemoryRandomAccessStream> CaptureSingleImage();
    }
}