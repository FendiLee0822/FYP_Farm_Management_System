using System;
using System.Collections.Generic;

#nullable disable

namespace Farm_Management_System.farm_Management_SystemModel
{
    public partial class Employee
    {
        public Employee()
        {
            Tasks = new HashSet<Task>();
        }

        public int EmployeeId { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
