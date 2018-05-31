using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebWww.Startup))]
namespace WebWww
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
