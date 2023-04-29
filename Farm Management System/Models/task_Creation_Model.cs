using Farm_Management_System.farm_Management_SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farm_Management_System.Models
{
    public class task_Creation_Model
    {
        public farm_Management_SystemModel.Task Task { get; set; }
        public IEnumerable<Field> Fields { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Plant> Plants { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<Admin> Admins { get; set; }
    }
}
