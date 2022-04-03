namespace ExamInDesktopUI.Library.Models
{
    public class ExamModel
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private object[] _alternatives;
        public object[] Alternatives
        {
            get { return _alternatives; }
            set { _alternatives = value; }
        }
    }
}