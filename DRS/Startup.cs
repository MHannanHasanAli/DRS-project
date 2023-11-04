using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DRS.Startup))]
namespace DRS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
           
        }
    }
}
