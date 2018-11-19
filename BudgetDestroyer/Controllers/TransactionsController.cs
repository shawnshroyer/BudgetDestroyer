using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BudgetDestroyer.Models;
using BudgetDestroyer.Helpers;

namespace BudgetDestroyer.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TransactionsHelper transactionsHelper = new TransactionsHelper();

        // GET: Transactions
        public ActionResult Index()
        {
            var transactions = db.Transactions.Include(t => t.EnteredBy).Include(t => t.HouseAccount).Include(t => t.TransactionType);
            return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.HouseAccountId = new SelectList(db.HouseAccounts, "Id", "Name");
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name");
            ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "Name");

            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HouseAccountId,TransactionTypeId,BudgetItemId,EnteredById,Discription,Date,Amount,Reconciled,ReconciledAmount")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.Reconciled = false;
                transaction.ReconciledAmount = 0.00M;
                transaction.Date = DateTime.Now;

                if (transaction.TransactionTypeId == 3) //Deposit
                {
                    transactionsHelper.AddToAccount(transaction.HouseAccountId, transaction.Amount);
                    transactionsHelper.AddToBudgetItem(transaction.BudgetItemId, transaction.Amount);
                }
                else if (transaction.TransactionTypeId == 4) //Withdraw
                {
                    //If amount is a negative number then the called functions will act accordingly.
                    transactionsHelper.SubtractFromAccount(transaction.HouseAccountId, transaction.Amount);
                    transactionsHelper.SubtractFromBudgetItem(transaction.BudgetItemId, transaction.Amount);

                    if (transaction.Amount > 0)
                    {
                        transaction.Amount *= -1;
                    }
                }

                db.Transactions.Add(transaction);
                db.SaveChanges();

                return RedirectToAction("Index", "Households");
            }

            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName", transaction.EnteredById);
            ViewBag.HouseAccountId = new SelectList(db.HouseAccounts, "Id", "Name", transaction.HouseAccountId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name", transaction.TransactionTypeId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName", transaction.EnteredById);
            ViewBag.HouseAccountId = new SelectList(db.HouseAccounts, "Id", "Name", transaction.HouseAccountId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name", transaction.TransactionTypeId);
            ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "Name", transaction.BudgetItemId);

            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseAccountId,TransactionTypeId,BudgetItemId,EnteredById,Discription,Date,Amount,Reconciled,ReconciledAmount")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                if (!transaction.Reconciled)
                {
                    //var oldBalance = db.Transactions.AsNoTracking().Where(t => t.Id == transaction.Id).FirstOrDefault().ReconciledAmount;
                    if (transaction.ReconciledAmount > 0)
                    {
                        transaction.Reconciled = true;
                    }
                }
                var oldTransaction = db.Transactions.AsNoTracking().FirstOrDefault(t => t.Id == transaction.Id);

                if (transaction.TransactionTypeId == 3 && transaction.TransactionTypeId != oldTransaction.TransactionTypeId) //Deposit
                {
                    //Logic change from withdraw to deposit (run add old ammount add new mount)
                    var modAmount = transaction.Amount + oldTransaction.Amount;

                    transactionsHelper.AddToAccount(transaction.HouseAccountId, modAmount);
                    transactionsHelper.AddToBudgetItem(transaction.BudgetItemId, modAmount);

                    transaction.Amount = oldTransaction.Amount;
                }
                else if (transaction.TransactionTypeId == 4 && transaction.TransactionTypeId != oldTransaction.TransactionTypeId) //Withdraw
                {
                    //Logic change from deposit to withdraw (run sub old and then new ammount)
                    decimal modAmount = transaction.Amount;
                    if (transaction.Amount < 0)
                    {
                        oldTransaction.Amount *= -1;
                        modAmount += oldTransaction.Amount;
                    }
                    else
                    {
                        modAmount += oldTransaction.Amount;
                        modAmount *= -1;

                        transaction.Amount *= -1;
                    }

                    //These will properly handle negative or poitive numbers.
                    transactionsHelper.SubtractFromAccount(transaction.HouseAccountId, modAmount);
                    transactionsHelper.SubtractFromBudgetItem(transaction.BudgetItemId, modAmount);
                }
                else if (transaction.TransactionTypeId == 3 && transaction.Amount != oldTransaction.Amount)  //Deposit
                {
                    if (transaction.Amount > oldTransaction.Amount)
                    {
                        var modAmount = transaction.Amount - oldTransaction.Amount;
                        transactionsHelper.AddToAccount(transaction.HouseAccountId, modAmount);
                        transactionsHelper.AddToBudgetItem(transaction.BudgetItemId, modAmount);
                    }
                    else if (transaction.Amount < oldTransaction.Amount)
                    {
                        var modAmount = oldTransaction.Amount - transaction.Amount;
                        transactionsHelper.SubtractFromAccount(transaction.HouseAccountId, modAmount);
                        transactionsHelper.SubtractFromBudgetItem(transaction.BudgetItemId, modAmount);
                    }
                }
                else if (transaction.TransactionTypeId == 4 && transaction.Amount != oldTransaction.Amount) //Withdraw
                {
                    if (transaction.Amount > 0)
                    {
                        transaction.Amount *= -1;
                    }

                    if (transaction.Amount > oldTransaction.Amount)
                    {
                        var modAmount = oldTransaction.Amount - transaction.Amount;
                        modAmount *= -1; //Add helpers don't turn negative numbers positive

                        transactionsHelper.AddToAccount(transaction.HouseAccountId, modAmount);
                        transactionsHelper.AddToBudgetItem(transaction.BudgetItemId, modAmount);
                    }
                    else if (transaction.Amount < oldTransaction.Amount)
                    {
                        var modAmount = oldTransaction.Amount - transaction.Amount;

                        transactionsHelper.SubtractFromAccount(transaction.HouseAccountId, modAmount);
                        transactionsHelper.SubtractFromBudgetItem(transaction.BudgetItemId, modAmount);
                    }
                }


                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Households");
            }
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName", transaction.EnteredById);
            ViewBag.HouseAccountId = new SelectList(db.HouseAccounts, "Id", "Name", transaction.HouseAccountId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name", transaction.TransactionTypeId);

            return View(transaction);
        }

        //// GET: Transactions/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Transaction transaction = db.Transactions.Find(id);
        //    if (transaction == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(transaction);
        //}

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);

            if (transaction.TransactionTypeId == 3) //Deposit
            {
                transactionsHelper.SubtractFromAccount(transaction.HouseAccountId, transaction.Amount);
                transactionsHelper.SubtractFromBudgetItem(transaction.BudgetItemId, transaction.Amount);
            }
            else //Withdraw
            {
                transactionsHelper.AddToAccount(transaction.HouseAccountId, transaction.Amount);
                transactionsHelper.AddToBudgetItem(transaction.BudgetItemId, transaction.Amount);
            }

            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Index", "Households");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
