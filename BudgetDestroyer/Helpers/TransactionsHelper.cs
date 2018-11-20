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
            if (amount < 0)
            {
                amount *= -1;
            }

            var account = db.HouseAccounts.Find(acctId);
            account.Balance += amount;

            db.Entry(account).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void AddToBudgetItem(int itemId, decimal amount)
        {
            if (amount < 0)
            {
                amount *= -1;
            }

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

        public void ChangeBudgetItem(int oldId, int newId, decimal amount)
        {
            var oldBudgetItem = db.BudgetItems.Find(oldId);
            var newBudgetItem = db.BudgetItems.Find(newId);

            if (newBudgetItem.BudgetId != oldBudgetItem.BudgetId)
            {
                var newBudget = db.Budgets.Find(newBudgetItem.BudgetId);
                var oldBudget = db.Budgets.Find(oldBudgetItem.BudgetId);

                newBudget.Amount += amount;
                oldBudget.Amount -= amount;

                db.Entry(newBudget).State = EntityState.Modified;
                db.Entry(oldBudget).State = EntityState.Modified;
            }

            newBudgetItem.Amount += amount;
            oldBudgetItem.Amount -= amount;

            db.Entry(oldBudgetItem).State = EntityState.Modified;
            db.Entry(newBudgetItem).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}