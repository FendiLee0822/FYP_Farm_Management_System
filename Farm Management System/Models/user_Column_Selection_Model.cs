using Farm_Management_System.farm_Management_SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farm_Management_System.Models
{
    public class user_Column_Selection_Model
    {
        public List<User> Users { get; set; }
        public List<string> selected_User_Attribute { get; set; }
    }
}
