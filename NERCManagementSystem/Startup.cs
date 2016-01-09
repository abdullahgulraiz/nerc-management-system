using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NERCManagementSystem.Startup))]
namespace NERCManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
