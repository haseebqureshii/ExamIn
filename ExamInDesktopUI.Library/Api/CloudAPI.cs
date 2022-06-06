using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExamInDesktopUI.Library.Api
{
    public class CloudAPI : ICloudApi
    {
        public async Task GetObject()
        {
            string url = @"http://127.0.0.1:5000/ObjectModel";
            var path = @"D:\FYP\ExamIn\ExamInDesktopUI.Library\images";
            List<string> Resultlist = new List<string>();
            TextWriter tw = new StreamWriter(@"D:\FYP\ExamIn\ExamInDesktopUI.Library\OBJ_RESULTS.txt", false, System.Text.Encoding.UTF8);
            HttpClient ApiClient = new HttpClient();

            foreach (string file in Directory.EnumerateFiles(path, "*.jpg"))
            {
                var stream = File.ReadAllBytes(file);
                MultipartFormDataContent content = new MultipartFormDataContent
                {
                    { new ByteArrayContent(stream), "image", file }
                };
                content.Headers.Add("encType", "multipart/form-data");

                using (HttpResponseMessage response = await ApiClient.PostAsync(url, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsAsync<string>();
                        tw.WriteLine(result);
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
                content.Dispose();
            }
            ApiClient.Dispose();
            tw.Close();
        }
        public async Task GetFace()
        {
            string url = @"http://127.0.0.1:5000/FaceModel";
            var path = @"D:\FYP\ExamIn\ExamInDesktopUI.Library\images";
            TextWriter tw = new StreamWriter(@"D:\FYP\ExamIn\ExamInDesktopUI.Library\FACE_RESULTS.txt", false, System.Text.Encoding.UTF8);
            HttpClient ApiClient = new HttpClient();

            foreach (string file in Directory.EnumerateFiles(path, "*.jpg"))
            {
                var stream = File.ReadAllBytes(file);
                MultipartFormDataContent content = new MultipartFormDataContent
                {
                    { new ByteArrayContent(stream), "image", file }
                };
                content.Headers.Add("encType", "multipart/form-data");
                using (HttpResponseMessage response = await ApiClient.PostAsync(url, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsAsync<string>();
                        tw.WriteLine(result);
                    }
                    else
                    {
                        new Exception("Bad_Input/Server_Error");
                    }
                }
                content.Dispose();
            }
            ApiClient.Dispose();
            tw.Close();
        }
        public async Task MonitorAudio()
        {
            string url = @"http://127.0.0.1:5000/AudioModel";
            HttpClient ApiClient = new HttpClient();

            using (HttpResponseMessage response = await ApiClient.GetAsync(url))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            ApiClient.Dispose();
        }
        public void DeleteImageFolder()
        {
            DirectoryInfo di = new DirectoryInfo(@"D:\FYP\ExamIn\ExamInDesktopUI.Library\images");
            foreach (FileInfo file in di.EnumerateFiles())
                file.Delete();

            foreach (DirectoryInfo dir in di.EnumerateDirectories())
                dir.Delete(true);
        }
    }
}
