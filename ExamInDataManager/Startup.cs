using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ExamInDataManager.Startup))]

namespace ExamInDataManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
