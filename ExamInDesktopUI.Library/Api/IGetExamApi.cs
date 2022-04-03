using ExamInDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExamInDesktopUI.Library.Api
{
    public interface IGetExamApi
    {
        Task<List<ExamModel>> GetAll();
    }
}
