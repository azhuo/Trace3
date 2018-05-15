using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Trace3.Startup))]
namespace Trace3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
