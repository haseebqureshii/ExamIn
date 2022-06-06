using ExamInDataManager.Library.DataAccess;
using ExamInDataManager.Library.Models;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Web.Http;

namespace ExamInDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        public UserModel GetById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();

            UserData _data = new UserData();

            return _data.GetUserById(userId).First();
        }

        public IHttpActionResult PostAsync(UserModel user)
        {
            user.Id = RequestContext.Principal.Identity.GetUserId();

            UserData _data = new UserData();
            try
            {
                var res = _data.PostUser(user);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
