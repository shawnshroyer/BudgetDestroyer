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
    using System.Collections.Generic;

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

            if (!context.Budgets.Any(b => b.Name == "Main Budget"))
            {
                List<Budget> budgets = new List<Budget>
                {
                    new Budget { Id = 1000, Name = "Main Budget", Descriptions = "Main Budget", HouseholdId = 1 },
                };

                List<BudgetItem> budgetItems = new List<BudgetItem>
                {
                    new BudgetItem { Id = 1000, Name = "Rent", Amount = 1000, Description = "Main Residence", BudgetId = 1000 },
                    new BudgetItem { Id = 1001, Name = "Utilities", Amount = 250, Description = "Run the House", BudgetId = 1000 },
                    new BudgetItem { Id = 1002, Name = "Clothing", Amount = 50, Description = "What to Wear", BudgetId = 1000 },
                    new BudgetItem { Id = 1003, Name = "Groceries", Amount = 1000, Description = "Need to Eat", BudgetId = 1000 },
                    new BudgetItem { Id = 1004, Name = "Entertainment", Amount = 150, Description = "Have Fun", BudgetId = 1000 },
                    new BudgetItem { Id = 1005, Name = "Food", Amount = 1000, Description = "Got to Eat Out at Times", BudgetId = 1000 },
                    new BudgetItem { Id = 1006, Name = "Charity", Amount = 20, Description = "Share the Wealth", BudgetId = 1000 }
                };

                List<HouseAccount> houseAccounts = new List<HouseAccount>
                {
                    new HouseAccount { Id = 1000, Name = "5/3 Bank Checking", Balance = 3000, HouseholdId = 1 },
                    new HouseAccount { Id = 1001, Name = "5/3 Bank Savings", Balance = 500, HouseholdId = 1 }
                };

                List<TransactionType> transactionTypes = new List<TransactionType>
                {
                    new TransactionType { Id = 1000, Name="Deposit"},
                    new TransactionType { Id = 1001, Name="Withdraw"},
                };

                List<Transaction> transactions = new List<Transaction>
                {
                    new Transaction { HouseAccountId = 1000, BudgetItemId = 1000, TransactionTypeId = 1001, EnteredById = "a39e858b-4a65-4f93-970d-976ec6392ae9", Name = "Rent", Amount = 850, Description = "Main Residence", Date = DateTime.Now},
                    new Transaction { HouseAccountId = 1000, BudgetItemId = 1001, TransactionTypeId = 1001, EnteredById = "a39e858b-4a65-4f93-970d-976ec6392ae9", Name = "Gas", Amount = 40, Description = "Piedmont NG",  Date = DateTime.Now },
                    new Transaction { HouseAccountId = 1000, BudgetItemId = 1001, TransactionTypeId = 1001, EnteredById = "a39e858b-4a65-4f93-970d-976ec6392ae9", Name = "Water", Amount = 35, Description = "City Water",  Date = DateTime.Now },
                    new Transaction { HouseAccountId = 1000, BudgetItemId = 1001, TransactionTypeId = 1001, EnteredById = "a39e858b-4a65-4f93-970d-976ec6392ae9", Name = "Electricity", Amount = 60, Description = "Duke Energy",  Date = DateTime.Now },
                    new Transaction { HouseAccountId = 1000, BudgetItemId = 1004, TransactionTypeId = 1001, EnteredById = "a39e858b-4a65-4f93-970d-976ec6392ae9", Name = "AMC Theater", Amount = 20, Description = "Moves and Snacks", Date = DateTime.Now },
                    new Transaction { HouseAccountId = 1000, BudgetItemId = 1003, TransactionTypeId = 1001, EnteredById = "a39e858b-4a65-4f93-970d-976ec6392ae9", Name = "Harris Teeter", Amount = 20, Description = "Snacks", Date = DateTime.Now },
                    new Transaction { HouseAccountId = 1000, BudgetItemId = 1003, TransactionTypeId = 1001, EnteredById = "a39e858b-4a65-4f93-970d-976ec6392ae9", Name = "Food Lion", Amount = 20, Description = "Meals", Date = DateTime.Now }
                };

                foreach (var budget in budgets)
                {
                    context.Budgets.Add(budget);
                }
                foreach (var budgetItem in budgetItems)
                {
                    context.BudgetItems.Add(budgetItem);
                }
                foreach (var houseAccount in houseAccounts)
                {
                    context.HouseAccounts.Add(houseAccount);
                }
                foreach (var transactionType in transactionTypes)
                {
                    context.TransactionTypes.Add(transactionType);
                }
                foreach (var transaction in transactions)
                {
                    context.Transactions.Add(transaction);
                }

                context.SaveChanges();
            }
        }
    }
}
