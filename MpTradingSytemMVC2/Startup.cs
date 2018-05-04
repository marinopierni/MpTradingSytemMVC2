using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MpTradingSytemMVC2.Startup))]
namespace MpTradingSytemMVC2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
