using ExamInDesktopUI.Models;
using System.Threading.Tasks;

namespace ExamInDesktopUI.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}