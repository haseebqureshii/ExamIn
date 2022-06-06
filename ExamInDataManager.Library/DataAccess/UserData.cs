using Dapper;
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

            var p = new { _id = Id };

            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "ExamInDataCS");

            return output;
        }

        public int PostUser(UserModel user)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var dictionary = new Dictionary<string, object>
            {
                { "@id", user.Id },
                { "@firstName", user.FirstName },
                { "@lastName", user.LastName },
                { "@email", user.EmailAddress },
                { "@createdDate", user.CreatedDate }
            };
            var p = new DynamicParameters(dictionary);

            return sql.SaveData("dbo.spUserAdd", p, "ExamInDataCS");
        }
    }
}
