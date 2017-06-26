using ADE_ManagementSystem.Models;
using ADE_ManagementSystem.Models.User;
using AutoMapper;
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
            Mapper.Initialize(x =>
            {
                x.CreateMap<UserViewModel, AspNetUser>();
                x.CreateMap<AspNetUser, UserViewModel>();

            });
        }
    }
}
