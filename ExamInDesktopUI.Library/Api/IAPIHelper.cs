using ExamInDesktopUI.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExamInDesktopUI.Helpers
{
    public interface IAPIHelper
    {
        HttpClient ApiClient { get; }
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}