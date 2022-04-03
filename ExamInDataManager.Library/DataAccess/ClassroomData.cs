using ExamInDataManager.Library.Internal.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamInDataManager.Library.DataAccess
{
    public class ClassroomData
    {
        public void SaveClassroom(string Id, string image)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var p = new { Id = Id };
            var q = new { image = image };

            sql.SaveData<dynamic, dynamic>("dbo.spClassroomDataSave", p, q, "ExamInData");
        }
    }
}
