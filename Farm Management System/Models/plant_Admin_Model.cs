using Farm_Management_System.farm_Management_SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farm_Management_System.Models
{
    public class plant_Admin_Model
    {
        public Plant Plant { get; set; }
        public IEnumerable<Admin> Admins { get; set; }
    }
}
