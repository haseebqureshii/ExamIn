using ExamInDataManager.Library.DataAccess;
using Microsoft.AspNet.Identity;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ExamInDataManager.Controllers
{
    [Authorize]
    public class UploadImageController : ApiController
    {
        public void PostById([FromBody]string value)
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            var image = Convert.FromBase64String(value);
            string hex = BitConverter.ToString(image);
            hex = hex.Replace("-", "");
            hex = "0x" + hex;

            UserData data = new UserData();

            data.SaveUserImage(userId, hex);
        }
    }
}
