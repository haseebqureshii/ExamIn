using System.Diagnostics;
using System.Threading.Tasks;

namespace ExamInDesktopUI.Library.Api
{
    public interface ICloudApi
    {
        void GetObject();
        void GetFace();
        void MonitorAudio();
    }
}