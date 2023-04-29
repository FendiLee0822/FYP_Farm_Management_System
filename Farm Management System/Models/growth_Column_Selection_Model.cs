using Farm_Management_System.farm_Management_SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farm_Management_System.Models
{
    public class growth_Column_Selection_Model
    {
        public List<Growth> Growths { get; set; }
        public List<string> selected_Growth_Attribute { get; set; }
    }
}
