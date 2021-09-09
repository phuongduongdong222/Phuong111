using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Events.web.Startup))]
namespace Events.web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
