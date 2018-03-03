using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hinoki.Startup))]
namespace Hinoki
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
