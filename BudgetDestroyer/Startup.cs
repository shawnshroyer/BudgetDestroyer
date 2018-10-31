using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BudgetDestroyer.Startup))]
namespace BudgetDestroyer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
