using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WeatherApplication.Startup))]
namespace WeatherApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
