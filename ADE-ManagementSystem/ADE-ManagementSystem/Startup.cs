using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ADE_ManagementSystem.Startup))]
namespace ADE_ManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
