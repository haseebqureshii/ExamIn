using ExamInDesktopUI.Helpers;
using ExamInDesktopUI.Library.Models;
using ExamInDesktopUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Storage.Streams;

namespace ExamInDesktopUI.Library.Api
{
    public class APIHelper : IAPIHelper
    {
        private readonly ILoggedInUserModel _loggedInUser;
        private readonly IMediaCaptureApi _mediaCapture;

        public APIHelper(ILoggedInUserModel loggedInUser, IMediaCaptureApi mediaCaptureApi)
        {
            InitializeClient();
            _loggedInUser = loggedInUser;
            _mediaCapture = mediaCaptureApi;
        }

        public HttpClient ApiClient { get; private set; }

        private void InitializeClient()
        {
            // Login API
            ApiClient = new HttpClient();
        }

        public async Task<bool> AuthenticateUserFace()
        {
            string url = @"http://127.0.0.1:5000/FaceModel";
            ApiClient.Dispose();
            ApiClient = new HttpClient
            {
                BaseAddress = new Uri(url)
            };
            ApiClient.DefaultRequestHeaders.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/jpeg"));

            var image = await _mediaCapture.CaptureSingleImage();
            var dr = new DataReader(image.GetInputStreamAt(0));
            var bytes = new byte[image.Size];
            await dr.LoadAsync((uint)image.Size);
            dr.ReadBytes(bytes);
            MultipartFormDataContent content = new MultipartFormDataContent
                {
                    { new ByteArrayContent(bytes), "image", "snapshot"}
                };
            content.Headers.Add("encType", "multipart/form-data");
            using (HttpResponseMessage response = await ApiClient.PostAsync(url, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsAsync<string>();
                    if (result.ToLower().Contains(_loggedInUser.EmailAddress) && result != "")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Bad_Input/Server_Error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        public async Task<bool> SaveNewUserFace(string username)
        {
            string url = @"http://127.0.0.1:5000/SaveImage";
            ApiClient.Dispose();
            ApiClient = new HttpClient
            {
                BaseAddress = new Uri(url)
            };
            ApiClient.DefaultRequestHeaders.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/jpeg"));

            var image = await _mediaCapture.CaptureSingleImage();
            var dr = new DataReader(image.GetInputStreamAt(0));
            var bytes = new byte[image.Size];
            await dr.LoadAsync((uint)image.Size);
            dr.ReadBytes(bytes);
            MultipartFormDataContent content = new MultipartFormDataContent
            {
                { new ByteArrayContent(bytes), "image", username}
            };
            content.Headers.Add("encType", "multipart/form-data");
            using (HttpResponseMessage response = await ApiClient.PostAsync(url, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<bool>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            string Api = ConfigurationManager.AppSettings["api"];
            ApiClient.Dispose();
            ApiClient = new HttpClient
            {
                BaseAddress = new Uri(Api)
            };
            ApiClient.DefaultRequestHeaders.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (HttpResponseMessage response = await ApiClient.PostAsync("/token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<bool> RegisterUser(string username, string password)
        {
            string Api = ConfigurationManager.AppSettings["api"];
            ApiClient.Dispose();
            ApiClient = new HttpClient
            {
                BaseAddress = new Uri(Api)
            };
            ApiClient.DefaultRequestHeaders.Clear();

            string raw = "{\"Email\":" + $"\"{username}\"" + 
                         ",\"Password\":" + $"\"{password}\"" + 
                         ",\"ConfirmPassword\":" + $"\"{password}\"" + '}';

            var content = new StringContent(raw, Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (HttpResponseMessage response = await ApiClient.PostAsync("/api/Account/Register", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<string> SetLoggedInUserInfo(string token, string firstname, string lastname, string username, string password)
        {
            ApiClient.DefaultRequestHeaders.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer { token }");

            string raw = "{\"Id\":" + $"\" \"" +
                         ",\"FirstName\":" + $"\"{firstname}\"" +
                         ",\"LastName\":" + $"\"{lastname}\"" +
                         ",\"EmailAddress\":" + $"\"{username}\"" +
                         ",\"CreatedDate\":" + $"\"{DateTime.UtcNow}\"" + '}';

            var content = new StringContent(raw, Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (HttpResponseMessage response = await ApiClient.PostAsync("/api/User", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return response.ReasonPhrase;
                }
            }
        }

        public async Task GetLoggedInUserInfo(string token)
        {
            ApiClient.DefaultRequestHeaders.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer { token }");

            using (HttpResponseMessage response = await ApiClient.GetAsync("/api/User"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();
                    _loggedInUser.CreatedDate = result.CreatedDate;
                    _loggedInUser.EmailAddress = result.EmailAddress;
                    _loggedInUser.FirstName = result.FirstName;
                    _loggedInUser.LastName = result.LastName;
                    _loggedInUser.Id = result.Id;
                    _loggedInUser.Token = token;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Logout()
        {
            if(_loggedInUser.Token?.Length > 0)
            {
                string Api = ConfigurationManager.AppSettings["api"];

                ApiClient.Dispose();
                ApiClient = new HttpClient
                {
                    BaseAddress = new Uri(Api)
                };
                ApiClient.DefaultRequestHeaders.Clear();
                ApiClient.DefaultRequestHeaders.Accept.Clear();
                ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer { _loggedInUser.Token }");

                using (HttpResponseMessage response = await ApiClient.PostAsync("/api/Account/Logout", null))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
        }
    }


}
