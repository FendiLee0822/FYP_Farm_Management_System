using System;
using System.Collections.Generic;

#nullable disable

namespace Farm_Management_System.farm_Management_SystemModel
{
    public partial class Financial
    {
        public int TransactionId { get; set; }
        public string TransactionName { get; set; }
        public int TransactionProject { get; set; }
        public double IncomeAmount { get; set; }
        public double ExpenseAmount { get; set; }
        public int AdminUpdate { get; set; }
        public DateTime TransactionUpdateTime { get; set; }

        public virtual Admin AdminUpdateNavigation { get; set; }
        public virtual Project TransactionProjectNavigation { get; set; }
    }
}
