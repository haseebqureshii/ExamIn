using System.Diagnostics;

namespace ExamInDesktopUI.Library.Api
{
    public class CloudAPI : ICloudApi
    {
        public void GetObject()
        {
            var processInfo = new ProcessStartInfo("D:\\FYP\\ExamIn\\ExamInDesktopUI.Library\\runObjReq.bat")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = "D:\\FYP\\ExamIn\\ExamInDesktopUI.Library"
            };

            var process = new Process();
            process.StartInfo = processInfo;
            process.Start();
            process.WaitForExit();
        }

        public void GetFace()
        {
            var processInfo = new ProcessStartInfo("D:\\FYP\\ExamIn\\ExamInDesktopUI.Library\\runFaceReq.bat")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = "D:\\FYP\\ExamIn\\ExamInDesktopUI.Library"
            };

            var process = new Process();
            process.StartInfo = processInfo;
            process.Start();
            process.WaitForExit();
        }

        public void MonitorAudio()
        {
            var processInfo = new ProcessStartInfo("D:\\FYP\\ExamIn\\ExamInDesktopUI.Library\\runAudioReq.bat")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = "D:\\FYP\\ExamIn\\ExamInDesktopUI.Library"
            };

            var process = new Process();
            process.StartInfo = processInfo;
            process.Start();
            process.WaitForExit();
        }
    }
}
