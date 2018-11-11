using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetDestroyer.Models
{
    public class HouseAccount
    {
        //Primary Key
        public int Id { get; set; }

        //Foreign Key
        public int HouseholdId { get; set; }

        public string Name { get; set; }
        public decimal Balance { get; set; }
        public decimal ReconciledBalace { get; set; }
        public bool DeleteAccount { get; set; }

        public virtual Household Household { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public HouseAccount()
        {
            Transactions = new HashSet<Transaction>();
        }
    }
}