using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcExam1.Startup))]
namespace MvcExam1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
