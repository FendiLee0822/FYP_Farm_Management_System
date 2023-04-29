using Farm_Management_System.farm_Management_SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farm_Management_System.Models
{
    public class financial_Creation_Model
    {
        public Financial Financials { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Admin> Admins { get; set; }
    }
}
