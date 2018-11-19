using BudgetDestroyer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BudgetDestroyer.Helpers
{
    public class TransactionsHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void AddToAccount(int acctId, decimal amount)
        {
            var account = db.HouseAccounts.Find(acctId);
            account.Balance += amount;

            db.Entry(account).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void AddToBudgetItem(int itemId, decimal amount)
        {
            var budgetItem = db.BudgetItems.Find(itemId);
            budgetItem.Amount += amount;

            var budget = db.Budgets.Find(budgetItem.BudgetId);
            budget.Amount += amount;

            db.Entry(budget).State = EntityState.Modified;
            db.Entry(budgetItem).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void SubtractFromAccount(int acctId, decimal amount)
        {
            var account = db.HouseAccounts.Find(acctId);

            if (amount < 0)
            {
                account.Balance += amount;
            }
            else
            {
                account.Balance -= amount;
            }

            db.Entry(account).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void SubtractFromBudgetItem(int itemId, decimal amount)
        {
            var budgetItem = db.BudgetItems.Find(itemId);
            var budget = db.Budgets.Find(budgetItem.BudgetId);

            if (amount < 0)
            {
                budgetItem.Amount += amount;
                budget.Amount += amount;
            }
            else
            {
                budgetItem.Amount -= amount;
                budget.Amount -= amount;
            }

            db.Entry(budget).State = EntityState.Modified;
            db.Entry(budgetItem).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}