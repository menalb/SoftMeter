using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SoftMeter.UIConsole.Startup))]
namespace SoftMeter.UIConsole
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();

            ConfigureAuth(app);
        }
    }
}
