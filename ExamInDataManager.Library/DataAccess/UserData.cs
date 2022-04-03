using ExamInDataManager.Library.Internal.DataAccess;
using ExamInDataManager.Library.Models;
using System.Collections.Generic;

namespace ExamInDataManager.Library.DataAccess
{
    public class UserData
    {
        public List<UserModel> GetUserById(string Id)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var p = new { Id = Id };

            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "ExamInData");

            return output;
        } 

        public void SaveUserImage(string Id, string image)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var p = new { Id = Id };
            var q = new { image = image };

            sql.SaveData<dynamic, dynamic>("dbo.spUserImageSave", p, q, "ExamInData");
        }
    }
}
