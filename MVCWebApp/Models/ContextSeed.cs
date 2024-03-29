﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MVCWebApp.Models
{

    public enum Roles
    {
        SuperAdmin,
        Admin,
        Moderator,
        Basic
    }

    public class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles

            //cast the enums directly to a string array
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                await roleManager.CreateAsync(new IdentityRole(item.ToString()));

            }
            //await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            //await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            //await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
            //await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));


        }
        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "superadmin@gmail.com",
                FirstName = "johnny",
                LastName = "mckinney",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Moderator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                }

            }
        }
    }
}
