using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farm_Management_System.Models
{
    public class task_Column_Selection_Model
    {
        //public List<Task> Tasks { get; set; }
        public List<Farm_Management_System.farm_Management_SystemModel.Task> Tasks { get; set; }
        public List<string> selected_Task_Attribute { get; set; }
    }
}
