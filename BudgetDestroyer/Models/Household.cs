using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetDestroyer.Models
{
    public class Household
    {
        //Primary Key
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<HouseAccount> HouseAccounts { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }

        public Household()
        {
            Budgets = new HashSet<Budget>();
            HouseAccounts = new HashSet<HouseAccount>();
            Members = new HashSet<ApplicationUser>();
            Invitations = new HashSet<Invitation>();
        }
    }

    public class Invitation
    {
        //Primary Key
        public int Id { get; set; }

        //ForiegnKey
        public int HouseholdId { get; set; }
        
        public Guid UniqueCode { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool Accepted { get; set; }

        public virtual Household Household { get; set; }
    }
}