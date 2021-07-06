using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WhatToDo.Startup))]
namespace WhatToDo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
