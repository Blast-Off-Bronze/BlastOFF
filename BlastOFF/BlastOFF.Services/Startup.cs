using Microsoft.Owin;

[assembly: OwinStartup(typeof(BlastOFF.Services.Startup))]

namespace BlastOFF.Services
{
    using Microsoft.Owin.Cors;

    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            this.ConfigureAuth(app);
        }
    }
}