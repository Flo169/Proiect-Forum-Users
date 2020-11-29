using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Proiect_Forum_Users.Startup))]
namespace Proiect_Forum_Users
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
