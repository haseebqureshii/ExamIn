using ExamInDesktopUI.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExamInDesktopUI.Helpers
{
    public interface IAPIHelper
    {
        HttpClient ApiClient { get; }
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task<bool> SaveNewUserFace(string username);
        Task<bool> RegisterUser(string username, string password);
        Task GetLoggedInUserInfo(string token);
        Task<string> SetLoggedInUserInfo(string token, string firstname, string lastname, string username, string password);
        Task<bool> AuthenticateUserFace();
        Task Logout();
    }
}