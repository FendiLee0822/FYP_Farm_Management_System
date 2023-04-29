using Farm_Management_System.farm_Management_SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farm_Management_System.Models
{
    public class inventory_User_Model
    {
        public Inventory Inventory { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
