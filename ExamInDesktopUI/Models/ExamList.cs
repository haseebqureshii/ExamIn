using System.Collections.Generic;

namespace ExamInDesktopUI.Models
{
    public class ExamList
    {
        private readonly List<Exam> _examList;

        public ExamList()
        {
            _examList = new List<Exam>();
        }


        //public IEnumerable<Classroom> GetClassroomsForUser(string email)
        //{
        // return; //implement exam lookup
        //}
    }
}
