namespace BudgetDestroyer.Migrations
{
    using BudgetDestroyer.Models;
    using BudgetDestroyer.Helpers;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BudgetDestroyer.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BudgetDestroyer.Models.ApplicationDbContext context)
        {
            //Create Roles
            var roleManger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "HoH"))
            {
                roleManger.Create(new IdentityRole { Name = "HoH" });
            }
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManger.Create(new IdentityRole { Name = "User" });
            }

            if (!context.Households.Any(h => h.Name == "Demo Household"))
            {
                Household household = new Household { Name = "Demo Household" };

                context.Households.Add(household);
                context.SaveChanges();
            }

            //Create Users
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "ShawnShroyer@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ShawnShroyer@mailinator.com",
                    Email = "ShawnShroyer@mailinator.com",
                    FirstName = "Shawn",
                    LastName = "Shroyer",
                    DisplayName = "Shawn Shroyer",
                    AvatarPath = ""
                }, "testUser123");

                // Assign a Role to a user
                var userId = userManager.FindByEmail("ShawnShroyer@mailinator.com").Id;
                userManager.AddToRole(userId, "HoH");
            }

            if (!context.Users.Any(u => u.Email == "JasonTwichell@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "JasonTwichell@mailinator.com",
                    Email = "JasonTwichell@mailinator.com",
                    FirstName = "Jason",
                    LastName = "Twichell",
                    DisplayName = "Twitch",
                    AvatarPath = ""
                }, "Abc&123!");

                // Assign a Role to a user
                var userId = userManager.FindByEmail("JasonTwichell@mailinator.com").Id;
                userManager.AddToRole(userId, "HoH");
            }


        }
    }
}
