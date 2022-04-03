using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
