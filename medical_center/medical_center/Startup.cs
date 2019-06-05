
using MED.Presentation;
using Microsoft.Owin;
using Owin;
using Startup = MED.Presentation.App_Start;


[assembly: OwinStartup(typeof(MED.Presentation.Startup))]
namespace MED.Presentation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
