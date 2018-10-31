using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetDestroyer.Models
{
    public class BudgetItem
    {
        //Primary Key
        public int Id { get; set; }

        //Foreign Keys
        public int BudgetId { get; set; }

        public decimal Amount { get; set; }
        public string Description { get; set; }

        public virtual Budget Budget { get; set; }

     
    }
}