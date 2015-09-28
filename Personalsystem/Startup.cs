using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Personalsystem.Startup))]
namespace Personalsystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
