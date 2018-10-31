using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetDestroyer.Models
{
    public class Budget
    {
        //Primary Key
        public int Id { get; set; }

        //Foreign Keys
        public int HouseholdId { get; set; }

        public string Name { get; set; }
        public string Descriptions { get; set; }

        public virtual Household Household { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }

        public Budget()
        {
            BudgetItems = new HashSet<BudgetItem>();
        }
    }
}