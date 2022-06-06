using System.Threading.Tasks;

namespace ExamInDesktopUI.Library.Api
{
    public interface ICloudApi
    {
        Task GetObject();
        Task GetFace();
        Task MonitorAudio();
        void DeleteImageFolder();
    }
}