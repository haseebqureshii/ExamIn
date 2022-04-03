using ExamInDataManager.Library.DataAccess;
using ExamInDataManager.Library.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace ExamInDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        public UserModel GetById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();

            UserData data = new UserData();

            return data.GetUserById(userId).FirstOrDefault();
        }
    }
}
