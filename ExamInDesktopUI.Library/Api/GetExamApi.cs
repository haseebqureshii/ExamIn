using ExamInDesktopUI.Helpers;
using ExamInDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExamInDesktopUI.Library.Api
{
    public class GetExamApi : IGetExamApi
    {
        private IAPIHelper _apiHelper;

        public GetExamApi(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public HttpClient ApiClient { get; private set; }

        public async Task<List<ExamModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(ConfigurationManager.AppSettings["examURL"]))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<ExamModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
