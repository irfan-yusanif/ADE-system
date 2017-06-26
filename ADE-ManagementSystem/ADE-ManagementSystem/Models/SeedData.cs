using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ADE_ManagementSystem.Models
{
    //public class SeedData : DbMigration
    //{
    //    protected override void Seed(ApplicationDbContext context)
    //    {
    //        Task.Run(async () => { await SeedAsync(context); }).Wait();
    //    }

    //    private async Task SeedAsync(ApplicationDbContext context)
    //    {
    //        var userManager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(context));
    //        var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole, int, ApplicationUserRole>(context));

    //        if (!roleManager.Roles.Any())
    //        {
    //            await roleManager.CreateAsync(new ApplicationRole { Name = ApplicationRole.AdminRoleName });
    //            await roleManager.CreateAsync(new ApplicationRole { Name = ApplicationRole.AffiliateRoleName });
    //        }

    //        if (!userManager.Users.Any(u => u.UserName == "shimmy"))
    //        {
    //            var user = new ApplicationUser
    //            {
    //                UserName = "shimmy",
    //                Email = "shimmy@gmail.com",
    //                EmailConfirmed = true,
    //                PhoneNumber = "0123456789",
    //                PhoneNumberConfirmed = true
    //            };

    //            await userManager.CreateAsync(user, "****");
    //            await userManager.AddToRoleAsync(user.Id, ApplicationRole.AdminRoleName);
    //        }
    //    }
    //}
}