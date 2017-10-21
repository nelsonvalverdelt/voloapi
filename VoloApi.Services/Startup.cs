using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using VoloApi.Services.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartup(typeof(VoloApi.Services.Startup))]

namespace VoloApi.Services
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }
        //Create User Administrator and User Roles
        private void CreateRolesAndUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Administrator"))
            {
                var role = new IdentityRole();
                role.Name = "Administrator";
                roleManager.Create(role);
            }

            var user = new ApplicationUser();
            user.UserName = "admin@email.com";
            user.Email = "admin@email.com";
            string password = "@Administrator";

            var createUser = userManager.Create(user, password);

            if (createUser.Succeeded)
            {
                var result = userManager.AddToRole(user.Id, "Administrator");
            }

            if (!roleManager.RoleExists("Developer"))
            {
                var role = new IdentityRole();
                role.Name = "Developer";
                roleManager.Create(role);
            }

        }
    }
}
