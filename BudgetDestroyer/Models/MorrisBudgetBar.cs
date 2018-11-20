using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetDestroyer.Models
{
    public class MorrisBudgetBar
    {
        public string Label { get; set; }
        public decimal Target { get; set; }
        public decimal Actual { get; set; }
    }
}