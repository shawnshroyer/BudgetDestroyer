using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetDestroyer.Models
{
    public class TransactionType
    {
        //Primary Key
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public TransactionType()
        {
            Transactions = new HashSet<Transaction>();
        }
    }
}